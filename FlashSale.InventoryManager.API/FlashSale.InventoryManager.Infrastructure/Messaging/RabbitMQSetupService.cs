using FlashSale.InventoryManager.Infrastructure.Messaging.Exchanges;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace FlashSale.InventoryManager.Infrastructure.Messaging
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
            throw new NotImplementedException();
        }
    }
}
