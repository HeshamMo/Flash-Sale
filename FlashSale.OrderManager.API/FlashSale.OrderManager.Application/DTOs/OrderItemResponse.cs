namespace FlashSale.OrderManager.Application.DTOs.Orders
{
    public class OrderItemResponse
    {
        public Guid ProductId { get; set; }

        public int Quantity { get; set; }
    }
}