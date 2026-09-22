using FlashSale.OrderManager.Domain.Enums;
using FlashSale.OrderManager.Domain.Models;

namespace FlashSale.OrderManager.Application.Interfaces
{
    public interface IOutboxRepository
    {
        Task AddAsync(OutBoxMessage message);
        Task<List<OutBoxMessage>> GetUnpublishedAsync(OutboxEventType eventType);
        Task<OutBoxMessage?> GetOutBoxMessageByIdAsync(Guid messageId);
        Task MarkAsPublishedAsync(OutBoxMessage message);
        Task MarkAsFailedAsync(OutBoxMessage message, string error);
        OutBoxMessage CreateOutBoxMessage(OutboxEventType eventType, object payload, DateTimeOffset occurredOnUtc);
        OutBoxMessage CreateOrderCreatedOutBoxMessage(Order order, DateTimeOffset occurredOnUtc);
    }
}