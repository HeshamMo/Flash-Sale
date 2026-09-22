using FlashSale.InventoryManager.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace FlashSale.InventoryManager.Infrastructure.Persistance.Configurations
{
    public class ProductConfiguration:IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Price)
                .HasPrecision(18, 2);

            builder.Property(p => p.Name).HasMaxLength(100).IsRequired();
            builder.HasMany(p => p.Transactions).WithOne(t => t.product);
        }
    }
}
