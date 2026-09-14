using FlashSale.OrderManager.Domain.Enums;

namespace FlashSale.OrderManager.Application.DTOs.Orders
{
    public class OrderResponse
    {
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }

        public OrderStatus Status { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public List<OrderItemResponse> Items { get; set; } = new();
    }
}