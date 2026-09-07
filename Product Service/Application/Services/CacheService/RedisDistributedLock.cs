
using ProductManager.Application.Chache;
using ProductManager.Application.MessagingQueue.Messages;
using ProductManager.Application.Services.CashService;
using StackExchange.Redis;
using System.Diagnostics;

public class RedisDistributedLock:IRedisDistributedLock
{
    private readonly IDatabase _db;
    private readonly ISubscriber _subscriber;



    public async Task<string?> AcquireAsync(
        string productId, string lockOrderId, TimeSpan lockExpiry, TimeSpan maxWait, CancellationToken ct = default)
    {
        var key = $"product:lock:{productId}";
        var channel = $"product:lock:release:{productId}";

        var sw = Stopwatch.StartNew();

        while(sw.Elapsed < maxWait)
        {
            if(await _db.StringSetAsync(key, lockOrderId, lockExpiry, When.NotExists))
                return lockOrderId;

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


    public async Task<bool> ReleaseAsync(string productId, string lockOrderId)
    {
        var key = $"product:lock:{productId}";
        var channel = $"product:lock:release:{productId}";
        var result = (long)await _db.ScriptEvaluateAsync(
            CacheConstants.ReleaseScript,
            new RedisKey[] { key, channel },
            new RedisValue[] { lockOrderId });
        return result == 1;
    }


    public Task<IEnumerable<string>> LockOrderProducts(OrderCreatedMessage orderCreated)
    {
        return await LockOrderProducts(orderCreated.OrderId.ToString(), orderCreated.OrderProducts.Select(p => p.ProductId.ToString()));
    }

    public async Task<IEnumerable<string>> LockOrderProducts(string orderId, IEnumerable<string> productIds)
    {
        var sortedProductIds = productIds.Select(id => id.ToString())
            .OrderByDescending(id => id)
            .Distinct()
            .ToList();
        var locksIds = new List<string>();
        try
        {

            foreach(var id in sortedProductIds)
            {
                var lockId = await AcquireAsync(
                    id, orderId.ToString(),
                    lockExpiry: CacheConstants.RedisLockExpiry, maxWait: CacheConstants.DefaultMaxWait);

                if(lockId is null)
                {

                    await ReleaseOrderProducts(orderId.ToString(), locksIds);
                    return [];
                }

                locksIds.Add(lockId);

            }

            return locksIds;
        }

        catch
        {

            await ReleaseOrderProducts(
               orderId.ToString(),
               locksIds);

            // Do some logging work in the future here !
            throw;
        }
    }


    public async Task<bool> ReleaseOrderProducts(
          string orderId,
          IEnumerable<string> productIds)
    {
        var allReleased = true;
        try
        {


            foreach(var productId in productIds)
            {
                var released = await ReleaseAsync(
                    productId,
                    orderId);

                if(released is not true)
                {
                    allReleased = false;
                }
            }

        }


        catch
        {

            // Redis itself failed.
            // Continue trying to release the other locks.
            allReleased = false;
        }

        return allReleased;
    }

}
