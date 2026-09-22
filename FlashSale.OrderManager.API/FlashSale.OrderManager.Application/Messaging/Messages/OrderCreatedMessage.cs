namespace FlashSale.OrderManager.Application.Messaging.Messages
{
    public class OrderCreatedMessage:IOrderMessage
    {
        public Guid OrderId { get; set; }
        public ICollection<OrderCreatedProduct> Products { get; set; }
    }
}
