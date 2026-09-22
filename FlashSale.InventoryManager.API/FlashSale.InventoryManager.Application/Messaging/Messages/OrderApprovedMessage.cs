namespace FlashSale.InventoryManager.Application.Messaging.Messages
{
    public class OrderApprovedMessage:IOrderMessage
    {
        public Guid OrderId { get; set; }


    }
}
