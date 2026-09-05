using RabbitMQ.Client;

namespace Product_Service.MessagingQueue.Exchanges
{
    public static class ExchangeDeclerations
    {

        public static async Task Declare(IChannel channel)
        {
            await channel.ExchangeDeclareAsync(
                exchange: RabbitMqConstants.EXCHANGE_ORDER_Topic,
                type: ExchangeType.Topic,
                durable: true
                );


            await channel.ExchangeDeclareAsync(
                exchange: RabbitMqConstants.EXCHANGE_ORDER_DLX,
                type: ExchangeType.Topic,
                durable: true
                );

        }
    }
}
