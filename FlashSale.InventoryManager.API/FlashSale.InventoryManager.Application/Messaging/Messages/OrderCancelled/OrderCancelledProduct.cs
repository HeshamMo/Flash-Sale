namespace FlashSale.InventoryManager.Application.Messaging.Messages.OrderCancelled
{
    public class OrderCancelledProduct
    {
        public Guid ProductId { get; set; }
        public int ProductQuantity { get; set; }
    }
}
