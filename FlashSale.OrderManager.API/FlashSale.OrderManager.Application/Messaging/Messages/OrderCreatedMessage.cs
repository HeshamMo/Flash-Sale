namespace FlashSale.OrderManager.Application.Messaging.Messages
{
    public class OrderCreatedMessage
    {
        public Guid OrderId { get; set; }
        public ICollection<OrderCreatedProduct> Products { get; set; }
    }
}
