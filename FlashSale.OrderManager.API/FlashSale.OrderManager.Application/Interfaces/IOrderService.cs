using FlashSale.OrderManager.Application.DTOs.Orders;

namespace FlashSale.OrderManager.Application.Interfaces
{

    public interface IOrderService
    {

        Task<IEnumerable<OrderResponse>> GetOrdersByUserIdAsync();

        Task<OrderResponse> CreateOrderAsync(
            CreateOrderRequest request);

        Task<OrderResponse> GetOrderByIdAsync(
            Guid orderId);

        Task<OrderResponse> CancelOrderAsync(
            Guid orderId);

        Task<OrderResponse> CompleteOrderAsync(
            Guid orderId);
    }
}