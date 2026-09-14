namespace FlashSale.OrderManager.Application.Messaging.Messages
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
