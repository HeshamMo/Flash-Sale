
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductManager.Application.MessagingQueue.Messages;
using ProductManager.Application.Services.ProductMessageServices;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Product_Service.MessagingQueue.Consumers
{
    public class OrderCreatedConsumer:BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConnection _connection;
        private IChannel _channel;
        public OrderCreatedConsumer(IServiceScopeFactory scopeFactory, IConnection connection)
        {
            _scopeFactory = scopeFactory;

            _connection = connection;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            _channel = await _connection.CreateChannelAsync();

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (sender, args) =>
            {

                try
                {

                    var jsonMessage = Encoding.UTF8.GetString(args.Body.ToArray());
                    var orderCreatedMessage = JsonSerializer.Deserialize<OrderCreatedMessage>(jsonMessage);

                    using var scope = _scopeFactory.CreateScope();

                    var _productMessageHandler = scope.ServiceProvider.GetRequiredService<IProductMessageHandler>();

                    await _productMessageHandler.HandleOrderCreated(orderCreatedMessage);

                    await _channel.BasicAckAsync(args.DeliveryTag, multiple: false);
                }

                catch(Exception ex)
                {

                    await _channel.BasicNackAsync(deliveryTag: args.DeliveryTag, multiple: false, requeue: false);

                    // Should Log!
                }



            };


            await _channel.BasicConsumeAsync(
                RabbitMqConstants.QUEUE_ORDER_CREATED,
                autoAck: false,
                consumer: consumer);


            await Task.Delay(Timeout.Infinite, stoppingToken);
        }


        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if(_channel is not null)
            {
                await _channel.CloseAsync();
                await _channel.DisposeAsync();
            }

            await base.StopAsync(cancellationToken);
        }
    }


}
