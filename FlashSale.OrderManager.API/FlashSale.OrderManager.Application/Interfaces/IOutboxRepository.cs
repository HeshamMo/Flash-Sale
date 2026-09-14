using FlashSale.OrderManager.Domain.Enums;
using FlashSale.OrderManager.Domain.Models;

namespace FlashSale.OrderManager.Application.Interfaces
{
    public interface IOutboxRepository
    {
        Task AddAsync(OutBoxMessage message);
        Task<List<OutBoxMessage>> GetUnpublishedAsync(int batchSize, int leaseSeconds = 30);
        Task<OutBoxMessage?> GetOutBoxMessageByIdAsync(Guid messageId);
        Task MarkAsPublishedAsync(OutBoxMessage message);
        Task MarkAsFailedAsync(OutBoxMessage message, string error, int maxRetries = 5);
        OutBoxMessage CreateOutBoxMessage(OutboxEventType eventType, object payload, DateTimeOffset occurredOnUtc);
    }
}