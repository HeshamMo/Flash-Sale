using FlashSale.OrderManager.Domain.Models;

namespace FlashSale.OrderManager.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(Guid orderId);

        Task AddAsync(Order order);

        Task UpdateAsync(Order order);

        Task<IEnumerable<Order>> GetAllOrders();
        Task<IEnumerable<Order>> GetOrdersByUserId(Guid userId);
    }
}