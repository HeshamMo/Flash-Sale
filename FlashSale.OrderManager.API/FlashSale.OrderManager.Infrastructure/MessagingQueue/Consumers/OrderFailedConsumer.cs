using FlashSale.OrderManager.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FlashSale.OrderManager.Infrastructure.MessagingQueue.Consumers
{
    internal class OrderFailedConsumer:BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConnection _connection;
        private IChannel _channel;
        public OrderFailedConsumer(IServiceScopeFactory scopeFactory, IConnection connection)
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


                    var orderFailedMessage = args.GetMessageBodyAsOrderFailedMessage();

                    using var scope = _scopeFactory.CreateScope();

                    var _orderMessageHandler = scope.ServiceProvider.GetRequiredService<IOrderMessageHandler>();

                    await _orderMessageHandler.HandleOrderFailed(orderFailedMessage);

                    await _channel.BasicAckAsync(args.DeliveryTag, multiple: false);
                }

                catch(Exception ex)
                {

                    await _channel.BasicNackAsync(deliveryTag: args.DeliveryTag, multiple: false, requeue: false);

                    // Should Log!
                }



            };


            await _channel.BasicConsumeAsync(
                RabbitMqConstants.QUEUE_ORDER_FAILED,
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

