using Microsoft.EntityFrameworkCore;

public class EstoqueContext : DbContext
{
    public EstoqueContext(DbContextOptions<EstoqueContext> options) : base(options) { }
    public DbSet<Produto> Produtos { get; set; }
}