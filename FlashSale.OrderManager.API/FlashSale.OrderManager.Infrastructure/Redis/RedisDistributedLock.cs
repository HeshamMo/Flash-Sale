
using FlashSale.OrderManager.Application.Interfaces;
using StackExchange.Redis;
using System.Diagnostics;

namespace FlashSale.OrderManager.Infrastructure.Redis
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


        public async Task<bool> ReleaseAsync(string lockType, string objectToLockId, string lockId)
        {
            var key = $"{lockType}:lock:{objectToLockId}";
            var channel = $"{lockType}:lock:release:{objectToLockId}";
            var result = (long)await _db.ScriptEvaluateAsync(
                RedisOptions.ReleaseScript,
                new RedisKey[] { key, channel },
                new RedisValue[] { lockId });
            return result == 1;
        }



    }
}
