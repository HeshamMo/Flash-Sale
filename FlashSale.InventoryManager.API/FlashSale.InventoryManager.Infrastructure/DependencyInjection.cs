using FlashSale.InventoryManager.Application.Inerfaces;
using FlashSale.InventoryManager.Application.Interfaces;
using FlashSale.InventoryManager.Application.Services.InventoryService;
using FlashSale.InventoryManager.Infrastructure.Messaging;
using FlashSale.InventoryManager.Infrastructure.Messaging.Consumers;
using FlashSale.InventoryManager.Infrastructure.Messaging.Consumers.MessageHandlers;
using FlashSale.InventoryManager.Infrastructure.Messaging.Publishers;
using FlashSale.InventoryManager.Infrastructure.Persistance;
using FlashSale.InventoryManager.Infrastructure.Persistance.Repositories;
using FlashSale.InventoryManager.Infrastructure.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using StackExchange.Redis;

namespace FlashSale.InventoryManager.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
    this IServiceCollection services,
    IConfiguration configuration)
        {
            services.AddSingleton<IRedisDistributedLock, RedisDistributedLock>();

            services.AddScoped<IInventoryService, InventoryService>();

            services.AddScoped<IInventoryPublisher, InventoryPublisher>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();


            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IOrderMessageHandler, OrderMessageHandler>();

            services.AddScoped<IInventoryPublisher, InventoryPublisher>();


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


            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var connectionString =
                    configuration.GetConnectionString("Redis");

                return ConnectionMultiplexer.Connect(connectionString!);
            });

            services.AddSingleton(sp =>
            {
                var multiplexer = sp.GetRequiredService<IConnectionMultiplexer>();
                return multiplexer.GetDatabase();
            });

            services.AddSingleton(sp =>
            {
                var multiplexer = sp.GetRequiredService<IConnectionMultiplexer>();
                return multiplexer.GetSubscriber();
            });


            services.AddHostedService<RabbitMQSetupService>();
            services.AddHostedService<OrderCreatedConsumer>();

            return services;
        }
    }
}
