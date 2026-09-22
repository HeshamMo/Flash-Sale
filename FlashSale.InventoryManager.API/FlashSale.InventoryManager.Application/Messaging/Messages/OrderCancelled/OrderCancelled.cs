namespace FlashSale.InventoryManager.Application.Messaging.Messages.OrderCancelled
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
