namespace ProductManager.Application.MessagingQueue.Messages
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
