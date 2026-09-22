
namespace FlashSale.InventoryManager.Application.Interfaces
{
    public interface IRedisDistributedLock
    {
        public Task<string?> AcquireAsync
            (string lockType, string objectToLockId, TimeSpan lockExpiry, TimeSpan maxWait, CancellationToken ct = default);

        public Task<bool> ReleaseAsync(string lockType, string objectToLockId, string lockId);


        public Task<Dictionary<string, string>> AcquireMultipleAsync(string lockType, IEnumerable<string> objectToLockIds, TimeSpan lockExpiry, TimeSpan maxWait, CancellationToken ct = default);

        public Task<bool> ReleaseMultipleAsync(string lockType, Dictionary<string, string> locks);

    }
}
