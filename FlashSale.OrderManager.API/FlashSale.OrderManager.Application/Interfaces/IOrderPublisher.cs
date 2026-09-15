using FlashSale.OrderManager.Application.Messaging.Messages;
using RabbitMQ.Client;

namespace FlashSale.OrderManager.Application.Interfaces
{
    public interface IOrderPublisher
    {

        public Task PublishOrderCreated(IChannel channel, OrderCreatedMessage message);
        public Task PublishOrderCancelled(IChannel channel, OrderCreatedMessage message);
    }
}
