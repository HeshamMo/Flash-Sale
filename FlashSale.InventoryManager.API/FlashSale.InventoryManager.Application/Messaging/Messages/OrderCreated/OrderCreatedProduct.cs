namespace FlashSale.InventoryManager.Application.Messaging.Messages.OrderCreated
{
    public class OrderCreatedProduct
    {
        public Guid ProductId { get; set; }
        public int ProductQuantity { get; set; }
    }
}
