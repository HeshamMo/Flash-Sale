namespace FlashSale.OrderManager.Application.Messaging.Messages
{
    public class OrderFailedMessage:IOrderMessage
    {
        public Guid OrderId { get; set; }
        public OrderFailedMessage()
        {

        }
        public OrderFailedMessage(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}
