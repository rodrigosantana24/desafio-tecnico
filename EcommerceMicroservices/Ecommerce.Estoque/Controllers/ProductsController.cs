using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly EstoqueContext _db;
    public ProductsController(EstoqueContext db) { _db = db; }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _db.Produtos.ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var p = await _db.Produtos.FindAsync(id);
        return p == null ? NotFound() : Ok(p);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Produto prod)
    {
        _db.Produtos.Add(prod);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = prod.Id }, prod);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Produto prod)
    {
        var existing = await _db.Produtos.FindAsync(id);
        if (existing == null) return NotFound();
        existing.Nome = prod.Nome;
        existing.Preco = prod.Preco;
        existing.Estoque = prod.Estoque;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _db.Produtos.FindAsync(id);
        if (existing == null) return NotFound();
        _db.Produtos.Remove(existing);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
