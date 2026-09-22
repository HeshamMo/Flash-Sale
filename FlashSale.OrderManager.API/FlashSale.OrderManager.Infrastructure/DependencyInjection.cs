using FlashSale.OrderManager.Application.Interfaces;
using FlashSale.OrderManager.Application.Services;
using FlashSale.OrderManager.Infrastructure.MessagingQueue;
using FlashSale.OrderManager.Infrastructure.MessagingQueue.Consumers;
using FlashSale.OrderManager.Infrastructure.MessagingQueue.Consumers.MessageHandlers;
using FlashSale.OrderManager.Infrastructure.MessagingQueue.Publishers;
using FlashSale.OrderManager.Infrastructure.MessagingQueue.Publishers.OutBox;
using FlashSale.OrderManager.Infrastructure.Persistance.Repositories;
using FlashSale.OrderManager.Infrastructure.Persistance.UnitsOfWork;
using FlashSale.OrderManager.Infrastructure.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using StackExchange.Redis;

namespace FlashSale.OrderManager.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
    this IServiceCollection services,
    IConfiguration configuration)
        {
            services.AddSingleton<IRedisDistributedLock, RedisDistributedLock>();

            services.AddScoped<IOrderService, OrderService>();

            services.AddScoped<IOutboxRepository, OutboxRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IOrderMessageHandler, OrderMessageHandler>();

            services.AddSingleton<IOrderPublisher, OrderPublisher>();

            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var redisConnection =
                    configuration.GetConnectionString("Redis");

                return ConnectionMultiplexer.Connect(redisConnection!);
            });

            services.AddSingleton<IConnection>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();

                var factory = new ConnectionFactory
                {
                    HostName = configuration["RabbitMQ:HostName"],
                    UserName = configuration["RabbitMQ:UserName"],
                    Password = configuration["RabbitMQ:Password"],
                    Port = int.Parse(configuration["RabbitMQ:Port"]!),
                };

                return factory.CreateConnectionAsync().GetAwaiter().GetResult();
            });



            services.AddHostedService<RabbitMQSetupService>();
            services.AddHostedService<OrderFailedConsumer>();
            services.AddHostedService<OrderApprovedConsumer>();

            services.AddHostedService<OrderCreatedPublisher>();
            services.AddHostedService<OrderCancelledPublisher>();



            return services;
        }
    }
}
