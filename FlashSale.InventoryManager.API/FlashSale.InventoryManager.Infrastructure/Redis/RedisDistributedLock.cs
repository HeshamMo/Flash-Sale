
using FlashSale.InventoryManager.Application.Interfaces;
using StackExchange.Redis;
using System.Diagnostics;

namespace FlashSale.InventoryManager.Infrastructure.Redis
{
    public class RedisDistributedLock:IRedisDistributedLock
    {
        private readonly IDatabase _db;
        private readonly ISubscriber _subscriber;


        public RedisDistributedLock(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
            _subscriber = redis.GetSubscriber();
        }


        public async Task<string?> AcquireAsync(string lockType, string objectToLockId, TimeSpan lockExpiry, TimeSpan maxWait, CancellationToken ct = default)
        {
            var key = $"{lockType}:lock:{objectToLockId}";
            var channel = $"{lockType}:lock:release:{objectToLockId}";

            var sw = Stopwatch.StartNew();
            var randomLockId = Guid.NewGuid().ToString();
            while(sw.Elapsed < maxWait)
            {
                if(await _db.StringSetAsync(key, randomLockId, lockExpiry, When.NotExists))
                    return randomLockId;

                var tcs = new TaskCompletionSource();
                var sub = await _subscriber.SubscribeAsync(RedisChannel.Literal(channel));
                sub.OnMessage(_ => tcs.TrySetResult());

                // fallback poll in case we missed the pub/sub message
                // (e.g. lock was released between our GET and our SUBSCRIBE)
                var remaining = maxWait - sw.Elapsed;
                var timeout = TimeSpan.FromMilliseconds(Math.Min(remaining.TotalMilliseconds, 500));


                await Task.WhenAny(tcs.Task, Task.Delay(timeout, ct));
                await sub.UnsubscribeAsync();
            }

            return null; // caller decides: fail, requeue, etc.
        }

        public async Task<Dictionary<string, string>> AcquireMultipleAsync(string lockType, IEnumerable<string> objectToLockIds,
            TimeSpan lockExpiry,
            TimeSpan maxWait,
            CancellationToken ct = default)
        {
            objectToLockIds = objectToLockIds.Distinct().OrderBy(id => id).ToList();

            var acquiredLockIds = new Dictionary<string, string>();

            try
            {
                foreach(var objectId in objectToLockIds)
                {
                    ct.ThrowIfCancellationRequested();

                    var lockId = await AcquireAsync(
                        lockType,
                        objectId,
                        lockExpiry,
                        maxWait,
                        ct);

                    if(lockId is null)
                    {
                        // Failed to acquire one of the locks.
                        // Release everything we already acquired.
                        await ReleaseMultipleAsync(
                            lockType,
                            acquiredLockIds);

                        return new Dictionary<string, string>();
                    }

                    acquiredLockIds.Add(objectId, lockId);
                }

                return acquiredLockIds;
            }
            catch
            {
                // Make sure we don't leave partial locks behind
                if(acquiredLockIds.Count > 0)
                {
                    await ReleaseMultipleAsync(
                        lockType,
                        acquiredLockIds);
                }

                //log;

                return new Dictionary<string, string>();
            }
        }

        public async Task<bool> ReleaseAsync(string lockType, string lockedObjectId, string lockId)
        {
            var key = $"{lockType}:lock:{lockedObjectId}";
            var channel = $"{lockType}:lock:release:{lockedObjectId}";
            var result = (long)await _db.ScriptEvaluateAsync(
                RedisOptions.ReleaseScript,
                new RedisKey[] { key, channel },
                new RedisValue[] { lockId });
            return result == 1;
        }

        public async Task<bool> ReleaseMultipleAsync(
                string lockType,
                Dictionary<string, string> acquiredLocks)
        {
            var allReleased = true;

            foreach(KeyValuePair<string, string> lockPair in acquiredLocks)
            {
                var released = await ReleaseAsync(
                    lockType,
                    lockPair.Key, lockPair.Value);

                if(!released)
                    allReleased = false;
            }

            return allReleased;
        }
    }
}
