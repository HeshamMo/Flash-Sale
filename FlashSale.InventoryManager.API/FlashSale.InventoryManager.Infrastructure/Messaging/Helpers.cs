using FlashSale.InventoryManager.Application.Messaging.Messages;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace FlashSale.InventoryManager.Infrastructure.Messaging
{
    public static class Helpers
    {



        public static T GetMessageBody<T>(this BasicDeliverEventArgs args)
        {
            var jsonMessage = Encoding.UTF8.GetString(args.Body.ToArray());

            return JsonSerializer.Deserialize<T>(
                jsonMessage,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
        }

        public static byte[] ConvertToMessageBodyBytes(this IOrderMessage message)
        {
            var jsonMessage = JsonSerializer.Serialize(message, message.GetType());
            var messageBody = Encoding.UTF8.GetBytes(jsonMessage);

            return messageBody;
        }



    }
}
