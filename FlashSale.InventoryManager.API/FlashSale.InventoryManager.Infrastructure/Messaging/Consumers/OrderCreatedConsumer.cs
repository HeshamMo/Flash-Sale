using FlashSale.InventoryManager.Application.Inerfaces;
using FlashSale.InventoryManager.Application.Messaging.Messages.OrderCreated;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
namespace FlashSale.InventoryManager.Infrastructure.Messaging.Consumers
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

                    var orderCreatedMessage = args.GetMessageBody<OrderCreatedMessage>();

                    using var scope = _scopeFactory.CreateScope();

                    var _orderMessageHandler = scope.ServiceProvider.GetRequiredService<IOrderMessageHandler>();

                    await _orderMessageHandler.HandleOrderCreated(orderCreatedMessage); ;

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
