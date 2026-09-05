namespace ProductManager.Application.MessagingQueue.Messages
{
    public class OrderItem
    {
        public Guid ProductId { get; set; }
        public int ProductQuantity { get; set; }
    }
}
