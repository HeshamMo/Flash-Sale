using FlashSale.OrderManager.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FlashSale.OrderManager.Infrastructure.Persistance
{
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

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<OrderProducts> OrderItems { get; set; }
        public virtual DbSet<OutBoxMessage> OutboxMessages { get; set; }



    }

}