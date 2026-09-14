namespace FlashSale.OrderManager.Application.Messaging.Messages
{
    public class OrderCreatedProduct
    {
        public Guid ProductId { get; set; }
        public int ProductQuantity { get; set; }
    }
}
