using FlashSale.OrderManager.Application.Interfaces;
using FlashSale.OrderManager.Application.Messaging.Messages;
using FlashSale.OrderManager.Domain.Enums;
using FlashSale.OrderManager.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace FlashSale.OrderManager.Infrastructure.MessagingQueue.Publishers.OutBox
{
    public class OrderCreatedPublisher:BackgroundService
    {

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConnection _connection;
        private readonly IOrderPublisher _orderPublisher;
        private IChannel _channel = null!;


        public OrderCreatedPublisher(
        IServiceScopeFactory scopeFactory,
        IConnection connection,
        IOrderPublisher orderPublisher)
        {
            _scopeFactory = scopeFactory;
            _connection = connection;
            _orderPublisher = orderPublisher;
        }


        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);
            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await ProcessBatch(stoppingToken);
        }

        private async Task ProcessBatch(CancellationToken stoppingToken)
        {

            while(!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                var _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                var outBoxMessageBatch = await _unitOfWork.Outbox.GetUnpublishedAsync(OutboxEventType.OrderCreated);

                if(outBoxMessageBatch.Any() is not true)
                {
                    await Task.Delay(TimeSpan.FromSeconds(2));
                    continue;
                }
                else
                {



                    foreach(OutBoxMessage outBoxMessage in outBoxMessageBatch)
                    {

                        try
                        {

                            var orderCreatedMessage = outBoxMessage.GetOrderMessageObj<OrderCreatedMessage>();

                            await _orderPublisher.PublishOrderCreated(_channel, orderCreatedMessage);

                            await _unitOfWork.Outbox.MarkAsPublishedAsync(outBoxMessage);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        catch(Exception ex)
                        {
                            await _unitOfWork.Outbox.MarkAsFailedAsync(outBoxMessage, ex.Message);
                        }
                    }



                }



            }
        }
    }
}
