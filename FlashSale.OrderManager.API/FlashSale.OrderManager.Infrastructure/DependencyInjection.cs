using FlashSale.OrderManager.Application.Interfaces;
using FlashSale.OrderManager.Infrastructure.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FlashSale.OrderManager.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
    this IServiceCollection services,
    IConfiguration configuration)
        {
            services.AddSingleton<IRedisDistributedLock, RedisDistributedLock>();


            return services;
        }
    }
}
