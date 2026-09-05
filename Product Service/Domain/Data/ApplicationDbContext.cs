using Microsoft.EntityFrameworkCore;
using ProductManager.Domain.Models;

namespace ProductManager.Domain.Data;

public class ApplicationDbContext:DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Product>()
    .Property(p => p.Price)
    .HasPrecision(18, 2);

        base.OnModelCreating(modelBuilder);
    }
    public DbSet<Product> Products { get; set; } = null!;
}
