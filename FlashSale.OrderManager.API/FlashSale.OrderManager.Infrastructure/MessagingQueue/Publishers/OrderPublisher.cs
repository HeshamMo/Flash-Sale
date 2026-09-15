using FlashSale.OrderManager.Application.Interfaces;
using FlashSale.OrderManager.Application.Messaging.Messages;
using RabbitMQ.Client;

namespace FlashSale.OrderManager.Infrastructure.MessagingQueue.Publishers
{
    public class OrderPublisher:IOrderPublisher
    {

        public async Task PublishOrderCreated(IChannel channel, OrderCreatedMessage message)
        {

            var basicProperties = new BasicProperties
            {
                DeliveryMode = DeliveryModes.Persistent
            };

            await channel.BasicPublishAsync(
            exchange: RabbitMqConstants.EXCHANGE_ORDER_Topic,
            routingKey: RabbitMqConstants.ROUTING_KEY_ORDER_CREATED,
            body: message.ConvertToMessageBodyBytes(),
            basicProperties: basicProperties,
            mandatory: false
            );
        }


        public async Task PublishOrderCancelled(IChannel channel, OrderCreatedMessage message)
        {
            var basicProperties = new BasicProperties
            {
                DeliveryMode = DeliveryModes.Persistent
            };

            await channel.BasicPublishAsync(
            exchange: RabbitMqConstants.EXCHANGE_ORDER_Topic,
            routingKey: RabbitMqConstants.QUEUE_ORDER_CANCELLED,
            body: message.ConvertToMessageBodyBytes(),
            basicProperties: basicProperties,
            mandatory: false
            );

        }



    }
}
