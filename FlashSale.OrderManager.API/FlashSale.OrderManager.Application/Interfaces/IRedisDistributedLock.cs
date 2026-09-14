
namespace FlashSale.OrderManager.Application.Interfaces
{
    public interface IRedisDistributedLock
    {
        public Task<string?> AcquireAsync
            (string lockType, string objectToLockId, TimeSpan lockExpiry, TimeSpan maxWait, CancellationToken ct = default);

        public Task<bool> ReleaseAsync(string lockType, string objectToLockId, string lockId);

    }
}
