using RabbitMQ.Client;

namespace FlashSale.InventoryManager.Infrastructure.Messaging
{
    public static class QueueDeclarations
    {
        public static async Task Declare(IChannel channel)
        {

            // =====================================================
            // ORDER CREATED
            // =====================================================

            var orderCreatedArguments =
                new Dictionary<string, object?>
                {
                    ["x-dead-letter-exchange"] =
                        RabbitMqConstants.EXCHANGE_ORDER_DLX,

                    ["x-dead-letter-routing-key"] =
                        RabbitMqConstants.ROUTING_KEY_ORDER_CREATED_DLQ
                };

            await channel.QueueDeclareAsync(
                queue: RabbitMqConstants.QUEUE_ORDER_CREATED,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: orderCreatedArguments
            );

            await channel.QueueBindAsync(
                queue: RabbitMqConstants.QUEUE_ORDER_CREATED,
                exchange: RabbitMqConstants.EXCHANGE_ORDER_Topic,
                routingKey: RabbitMqConstants.ROUTING_KEY_ORDER_CREATED
            );


            // =====================================================
            // ORDER CANCELLED
            // =====================================================

            var orderCancelledArguments =
                new Dictionary<string, object?>
                {
                    ["x-dead-letter-exchange"] =
                        RabbitMqConstants.EXCHANGE_ORDER_DLX,

                    ["x-dead-letter-routing-key"] =
                        RabbitMqConstants.ROUTING_KEY_ORDER_CANCELLED_DLQ
                };

            await channel.QueueDeclareAsync(
                queue: RabbitMqConstants.QUEUE_ORDER_CANCELLED,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: orderCancelledArguments
            );

            await channel.QueueBindAsync(
                queue: RabbitMqConstants.QUEUE_ORDER_CANCELLED,
                exchange: RabbitMqConstants.EXCHANGE_ORDER_Topic,
                routingKey: RabbitMqConstants.ROUTING_KEY_ORDER_CANCELLED
            );


            // =====================================================
            // ORDER FAILED
            // =====================================================

            var orderFailedArguments =
                new Dictionary<string, object?>
                {
                    ["x-dead-letter-exchange"] =
                        RabbitMqConstants.EXCHANGE_ORDER_DLX,

                    ["x-dead-letter-routing-key"] =
                        RabbitMqConstants.ROUTING_KEY_ORDER_FAILED_DLQ
                };

            await channel.QueueDeclareAsync(
                queue: RabbitMqConstants.QUEUE_ORDER_FAILED,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: orderFailedArguments
            );

            await channel.QueueBindAsync(
                queue: RabbitMqConstants.QUEUE_ORDER_FAILED,
                exchange: RabbitMqConstants.EXCHANGE_ORDER_Topic,
                routingKey: RabbitMqConstants.ROUTING_KEY_ORDER_FAILED
            );


            // =====================================================
            // ORDER APPROVED
            // =====================================================

            var orderApprovedArguments =
                new Dictionary<string, object?>
                {
                    ["x-dead-letter-exchange"] =
                        RabbitMqConstants.EXCHANGE_ORDER_DLX,

                    ["x-dead-letter-routing-key"] =
                        RabbitMqConstants.ROUTING_KEY_ORDER_APPROVED_DLQ
                };

            await channel.QueueDeclareAsync(
                queue: RabbitMqConstants.QUEUE_ORDER_Approved,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: orderApprovedArguments
            );

            await channel.QueueBindAsync(
                queue: RabbitMqConstants.QUEUE_ORDER_Approved,
                exchange: RabbitMqConstants.EXCHANGE_ORDER_Topic,
                routingKey: RabbitMqConstants.ROUTING_KEY_ORDER_APPROVED
            );





            //-------------------------------------------------
            //-------------------------------------------------
            //-------------------------------------------------



            // =====================================================
            // ORDER CREATED DLQ
            // =====================================================

            await channel.QueueDeclareAsync(
                queue: RabbitMqConstants.QUEUE_ORDER_CREATED_DLQ,
                durable: true,
                exclusive: false,
                autoDelete: false
            );

            await channel.QueueBindAsync(
                queue: RabbitMqConstants.QUEUE_ORDER_CREATED_DLQ,
                exchange: RabbitMqConstants.EXCHANGE_ORDER_DLX,
                routingKey: RabbitMqConstants.ROUTING_KEY_ORDER_CREATED_DLQ
            );


            // =====================================================
            // ORDER CANCELLED DLQ
            // =====================================================

            await channel.QueueDeclareAsync(
                queue: RabbitMqConstants.QUEUE_ORDER_CANCELLED_DLQ,
                durable: true,
                exclusive: false,
                autoDelete: false
            );

            await channel.QueueBindAsync(
                queue: RabbitMqConstants.QUEUE_ORDER_CANCELLED_DLQ,
                exchange: RabbitMqConstants.EXCHANGE_ORDER_DLX,
                routingKey: RabbitMqConstants.ROUTING_KEY_ORDER_CANCELLED_DLQ
            );


            // =====================================================
            // ORDER FAILED DLQ
            // =====================================================

            await channel.QueueDeclareAsync(
                queue: RabbitMqConstants.QUEUE_ORDER_FAILED_DLQ,
                durable: true,
                exclusive: false,
                autoDelete: false
            );

            await channel.QueueBindAsync(
                queue: RabbitMqConstants.QUEUE_ORDER_FAILED_DLQ,
                exchange: RabbitMqConstants.EXCHANGE_ORDER_DLX,
                routingKey: RabbitMqConstants.ROUTING_KEY_ORDER_FAILED_DLQ
            );


            // =====================================================
            // ORDER APPROVED DLQ
            // =====================================================

            await channel.QueueDeclareAsync(
                queue: RabbitMqConstants.QUEUE_ORDER_APPROVED_DLQ,
                durable: true,
                exclusive: false,
                autoDelete: false
            );

            await channel.QueueBindAsync(
                queue: RabbitMqConstants.QUEUE_ORDER_APPROVED_DLQ,
                exchange: RabbitMqConstants.EXCHANGE_ORDER_DLX,
                routingKey: RabbitMqConstants.ROUTING_KEY_ORDER_APPROVED_DLQ);


        }
    }
}