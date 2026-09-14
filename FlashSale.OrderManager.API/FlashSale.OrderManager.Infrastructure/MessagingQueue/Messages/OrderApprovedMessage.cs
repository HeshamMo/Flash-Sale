namespace FlashSale.OrderManager.Infrastructure.MessagingQueue.Messages
{
    public class OrderApprovedMessage
    {
        public Guid OrderId { get; set; }

        public OrderApprovedMessage(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}
