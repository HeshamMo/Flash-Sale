using Product_Service.MessagingQueue;
using ProductManager.Application.MessagingQueue.Messages;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace ProductManager.Application.Services.ProductMessageServices.ProductPublisher
{
    public class ProductPublisher:IProductPublisher
    {
        public async Task publishOrderApproved(IChannel channel, Guid orderId)
        {
            var basicProperties = new BasicProperties()
            {
                DeliveryMode = DeliveryModes.Persistent
            };

            await channel.BasicPublishAsync(
                exchange: RabbitMqConstants.EXCHANGE_ORDER_Topic,
                routingKey: RabbitMqConstants.ROUTING_KEY_ORDER_APPROVED,
                body: ConvertObjectToMQBody(new OrderApprovedMessage(orderId)),
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

            await channel.BasicPublishAsync(
                  exchange: RabbitMqConstants.EXCHANGE_ORDER_Topic,
                  routingKey: RabbitMqConstants.ROUTING_KEY_ORDER_FAILED,
                  body: ConvertObjectToMQBody(new OrderFailedMessage(orderId))
                  , basicProperties: basicProperties,
                  mandatory: false
                  );
        }



        public byte[] ConvertObjectToMQBody(object obj)
        {
            return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj));
        }
    }
}
