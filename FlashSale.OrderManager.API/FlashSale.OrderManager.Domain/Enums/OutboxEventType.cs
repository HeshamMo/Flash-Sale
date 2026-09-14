namespace FlashSale.OrderManager.Domain.Enums
{
    public enum OutboxEventType
    {

        OrderCreated = 0
         , OrderUpdated = 1,
        OrderCancelled = 2
    }
}
