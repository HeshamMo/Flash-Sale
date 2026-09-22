using FlashSale.InventoryManager.Application.Dtos;
using FlashSale.InventoryManager.Application.Messaging.Messages.OrderCancelled;
using FlashSale.InventoryManager.Application.Messaging.Messages.OrderCreated;
using FlashSale.InventoryManager.Domain.Models;

namespace FlashSale.InventoryManager.Application.Inerfaces
{

    public interface IInventoryService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();

        Task<ProductDto?> GetByIdAsync(Guid id);

        Task<ProductDto> CreateAsync(ProductDto dto);

        Task<Product> UpdateAsync(ProductDto dto);

        Task<bool> DeleteAsync(Guid id);

        Task<bool> ReserveStockAsync(
            Guid orderId,
            ICollection<OrderCreatedProduct> orderProductList);

        Task<bool> ReleaseStockAsync(
            Guid orderId,
            ICollection<OrderCancelledProduct> orderProductList);



    }
}
