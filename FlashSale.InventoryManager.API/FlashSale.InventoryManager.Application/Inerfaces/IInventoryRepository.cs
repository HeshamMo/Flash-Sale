using FlashSale.InventoryManager.Application.Messaging.Messages.OrderCancelled;
using FlashSale.InventoryManager.Application.Messaging.Messages.OrderCreated;
using FlashSale.InventoryManager.Domain.Models;

namespace FlashSale.InventoryManager.Infrastructure.Persistance.Repositories
{
    public interface IInventoryRepository
    {
        Task<Product> CreateAsync(Product product);

        Task<Product?> GetByIdAsync(Guid id);

        Task<IEnumerable<Product>> GetAllAsync();

        Task UpdateAsync(Product product);

        Task<bool> DeleteAsync(Guid id);

        Task<bool> ReserveStockAsync(
            Guid orderId,
            ICollection<OrderCreatedProduct> orderItems);

        Task<bool> ReleaseStockAsync(
            Guid orderId,
            ICollection<OrderCancelledProduct> orderItems);
    }
}
