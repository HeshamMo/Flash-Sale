using FlashSale.OrderManager.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlashSale.OrderManager.Infrastructure.Persistance.Configurations
{
    public class OutboxMessageConfiguration
    :IEntityTypeConfiguration<OutBoxMessage>
    {
        public void Configure(EntityTypeBuilder<OutBoxMessage> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.EventType)
                .HasConversion<string>()
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Payload)
                .IsRequired();

            builder.Property(x => x.OccurredOnUtc)
                .IsRequired();

            builder.Property(x => x.PublishedOnUtc)
                .IsRequired(false);

            builder.Property(x => x.RetryCount)
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(x => x.Error)
                .IsRequired(false);

            builder.HasIndex(x => x.PublishedOnUtc);
        }


    }
}
