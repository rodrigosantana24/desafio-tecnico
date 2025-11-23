using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Vendas.Models;
using System.Text.Json;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly VendasContext _db;
    private readonly RabbitMqService _mq;
    private readonly IHttpClientFactory _httpFactory;

    public OrdersController(VendasContext db, RabbitMqService mq, IHttpClientFactory httpFactory)
    {
        _db = db;
        _mq = mq;
        _httpFactory = httpFactory;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _db.Orders.ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var o = await _db.Orders.FindAsync(id);
        return o == null ? NotFound() : Ok(o);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Order order)
    {
        // validate stock via Estoque service
        var client = _httpFactory.CreateClient();
        // Use Docker service hostname to call Inventory when running in containers
        var resp = await client.GetAsync("http://estoque:5001/api/products/" + order.ProductId);
        if (!resp.IsSuccessStatusCode) return BadRequest("Product not found");
        var json = await resp.Content.ReadAsStringAsync();
        var prod = JsonSerializer.Deserialize<ProdutoDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if (prod == null || prod.Estoque < order.Quantity) return BadRequest("Insufficient stock");

        order.Status = "Confirmed";
        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        _mq.SendMessage(new { ProductId = order.ProductId, Quantity = order.Quantity });

        return CreatedAtAction(nameof(Get), new { id = order.Id }, order);
    }

    private class ProdutoDto { public int Id { get; set; } public int Estoque { get; set; } }
}
