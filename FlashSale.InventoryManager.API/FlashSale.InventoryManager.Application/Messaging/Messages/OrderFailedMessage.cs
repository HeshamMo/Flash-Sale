namespace FlashSale.InventoryManager.Application.Messaging.Messages
{
    public class OrderFailedMessage:IOrderMessage
    {
        public Guid OrderId { get; set; }

    }
}
