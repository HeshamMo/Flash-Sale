using FlashSale.InventoryManager.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlashSale.InventoryManager.Infrastructure.Persistance.Configurations
{
    public class InventoryTransactionConfiguration:IEntityTypeConfiguration<InventoryTransaction>
    {
        public void Configure(EntityTypeBuilder<InventoryTransaction> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedNever();

            builder.Property(x => x.ProductId)
                .IsRequired();

            builder.Property(x => x.OrderId)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.Quantity)
                .IsRequired();

            builder.Property(x => x.Type)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(50);


            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.HasIndex(x => x.ProductId);

            builder.HasIndex(x => x.OrderId);

            builder.HasOne(p => p.product).WithMany(p => p.Transactions);
        }
    }
}
