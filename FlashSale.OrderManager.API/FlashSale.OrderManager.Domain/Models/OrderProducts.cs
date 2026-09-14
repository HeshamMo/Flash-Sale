namespace FlashSale.OrderManager.Domain.Models
{
    public class OrderProducts
    {

        public Guid Id { get; set; }

        public Guid OrderId { get; set; }

        public Guid ProductId { get; set; }
        public int Quantity { get; set; }

        public Order Order { get; set; } = null!;
    }
}
