namespace FlashSale.InventoryManager.Application.Messaging.Messages.OrderCreated
{
    public class OrderCreatedMessage:IOrderMessage
    {
        public Guid OrderId { get; set; }
        public ICollection<OrderCreatedProduct> Products { get; set; }
    }
}
