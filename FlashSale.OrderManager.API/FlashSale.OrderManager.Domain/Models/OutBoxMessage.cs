using FlashSale.OrderManager.Domain.Enums;
namespace FlashSale.OrderManager.Domain.Models
{

    public class OutBoxMessage
    {
        public Guid Id { get; set; }

        public OutboxEventType EventType { get; set; }

        public string Payload { get; set; } = null!;

        public DateTimeOffset OccurredOnUtc { get; set; }

        public DateTimeOffset? PublishedOnUtc { get; set; }

        public int RetryCount { get; set; }

        public string? Error { get; set; }

        public DateTimeOffset? LockedUntilUtc { get; set; }
    }
}
