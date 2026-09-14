using FlashSale.OrderManager.Domain.Models;

namespace FlashSale.OrderManager.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(Guid orderId);

        Task AddAsync(Order order);

        Task UpdateAsync(Order order);


        Task<List<Order>> GetOrdersByUserId(Guid userId);
    }
}