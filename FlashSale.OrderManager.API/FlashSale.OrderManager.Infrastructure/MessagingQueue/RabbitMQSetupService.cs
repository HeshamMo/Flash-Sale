using FlashSale.OrderManager.Infrastructure.MessagingQueue.Exchanges;
using FlashSale.OrderManager.Infrastructure.MessagingQueue.Queues;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace FlashSale.OrderManager.Infrastructure.MessagingQueue
{
    public class RabbitMQSetupService:IHostedService
    {


        private readonly IConnection _connection;

        public RabbitMQSetupService(IConnection connection)
        {
            _connection = connection;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await using var channel = await _connection.CreateChannelAsync();

            await ExchangeDeclerations.Declare(channel);
            await QueueDeclarations.Declare(channel);

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
