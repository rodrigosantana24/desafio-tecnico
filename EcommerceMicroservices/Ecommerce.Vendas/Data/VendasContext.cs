using Microsoft.EntityFrameworkCore;
using Ecommerce.Vendas.Models;

public class VendasContext : DbContext
{
    public VendasContext(DbContextOptions<VendasContext> options) : base(options) { }
    public DbSet<Order> Orders { get; set; }
}
