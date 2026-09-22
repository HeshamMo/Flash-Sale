namespace FlashSale.OrderManager.Application.DTOs.Orders
{
    public class CreateOrderRequest
    {
        public List<CreateOrderItemRequest> Products { get; set; } = new();
    }
}