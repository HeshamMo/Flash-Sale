using FlashSale.OrderManager.Domain.Enums;
namespace FlashSale.OrderManager.Domain.Models
{
    public class Order
    {
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }

        public OrderStatus Status { get; set; }

        public DateTimeOffset CreatedAtUtc { get; set; }

        public ICollection<OrderItem> Items { get; set; }
            = new List<OrderItem>();

    }
}
