namespace FlashSale.OrderManager.Infrastructure.Redis
{
    public static class RedisOptions
    {

        public static readonly TimeSpan DefaultMaxWait =
            TimeSpan.FromSeconds(5);

        public static readonly TimeSpan RedisLockExpiry =
            TimeSpan.FromSeconds(30);



        public const string ReleaseLockFunctionName
            = "release_lock";


        public const string ReleaseScript = """
        if redis.call("GET", KEYS[1]) == ARGV[1] then
            redis.call("DEL", KEYS[1])
            redis.call("PUBLISH", KEYS[2], "released")
            return 1
        end
        return 0
        """;

    }
}
