namespace FlashSale.OrderManager.Application.Messaging.Messages
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
