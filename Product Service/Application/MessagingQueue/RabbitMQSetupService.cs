using Microsoft.Extensions.Hosting;
using Product_Service.MessagingQueue.Exchanges;
using Product_Service.MessagingQueue.Queues;
using RabbitMQ.Client;

namespace Product_Service.MessagingQueue
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
