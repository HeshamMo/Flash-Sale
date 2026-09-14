namespace FlashSale.OrderManager.Infrastructure.MessagingQueue.Messages
{
    public class OrderCreatedMessage
    {
        public Guid OrderId { get; set; }
        public ICollection<OrderItem> OrderProducts { get; set; }
    }
}
