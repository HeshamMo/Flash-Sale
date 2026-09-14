namespace FlashSale.OrderManager.Infrastructure.MessagingQueue.Messages
{
    public class OrderFailedMessage
    {
        public Guid OrderId { get; set; }
        public OrderFailedMessage(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}
