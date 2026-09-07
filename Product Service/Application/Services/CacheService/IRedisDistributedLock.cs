using ProductManager.Application.MessagingQueue.Messages;

namespace ProductManager.Application.Services.CashService
{
    public interface IRedisDistributedLock
    {
        Task<string?> AcquireAsync(
      string productId, string lockOrderId, TimeSpan lockExpiry, TimeSpan maxWait, CancellationToken ct = default);

        Task<bool> ReleaseAsync(string productId, string lockId);

        public Task<IEnumerable<string>> LockOrderProducts(OrderCreatedMessage orderCreated);
        public Task<IEnumerable<string>> LockOrderProducts(string orderId, IEnumerable<string> productIds);
        public Task<bool> ReleaseOrderProducts(string orderId, IEnumerable<string> productIds);


    }
}
