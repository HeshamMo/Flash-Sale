namespace FlashSale.OrderManager.Domain.Enums
{
    public enum OrderStatus
    {
        Pending = 0,
        StockReserved = 1,
        StockFailed = 2,
        Cancelled = 3,
        Paid = 4,
        Completed = 5
    }
}
