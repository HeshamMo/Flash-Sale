using FlashSale.InventoryManager.Application.Interfaces;
using FlashSale.InventoryManager.Application.Messaging.Messages;
using RabbitMQ.Client;

namespace FlashSale.InventoryManager.Infrastructure.Messaging.Publishers
{
    public class InventoryPublisher:IInventoryPublisher
    {
        public async Task publishOrderApproved(IChannel channel, Guid orderId)
        {
            var basicProperties = new BasicProperties()
            {
                DeliveryMode = DeliveryModes.Persistent
            };
            var message = new OrderApprovedMessage() { OrderId = orderId };
            await channel.BasicPublishAsync(
                exchange: RabbitMqConstants.EXCHANGE_ORDER_Topic,
                routingKey: RabbitMqConstants.ROUTING_KEY_ORDER_APPROVED,
                body: message.ConvertToMessageBodyBytes(),
                basicProperties: basicProperties,
                mandatory: false
                );
        }

        public async Task PublishOrderFail(IChannel channel, Guid orderId)
        {
            var basicProperties = new BasicProperties()
            {
                DeliveryMode = DeliveryModes.Persistent
            };
            var message = new OrderFailedMessage() { OrderId = orderId };

            await channel.BasicPublishAsync(
                  exchange: RabbitMqConstants.EXCHANGE_ORDER_Topic,
                  routingKey: RabbitMqConstants.ROUTING_KEY_ORDER_FAILED,
                  body: message.ConvertToMessageBodyBytes()
                  , basicProperties: basicProperties,
                  mandatory: false
                  );
        }



    }
}
