using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Product_Service.Models;

namespace Product_Service.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; } = null!;
}
