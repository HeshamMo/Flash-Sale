namespace FlashSale.OrderManager.Application.Options
{
    public class OutboxOptions
    {
        public int BatchSize { get; set; } = 20;
        public int LeaseSeconds { get; set; } = 30;
        public int MaxRetries { get; set; } = 5;
    }
}
