using FlashSale.InventoryManager.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FlashSale.InventoryManager.Infrastructure.Persistance;

public class ApplicationDbContext:DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {


        modelBuilder.ApplyConfigurationsFromAssembly(
      typeof(ApplicationDbContext).Assembly);


        base.OnModelCreating(modelBuilder);
    }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<InventoryTransaction> InventoryTransactions { get; set; }
}
