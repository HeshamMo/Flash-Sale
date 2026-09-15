using FlashSale.OrderManager.Application.Messaging.Messages;
using FlashSale.OrderManager.Domain.Models;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace FlashSale.OrderManager.Infrastructure.MessagingQueue
{
    public static class Helpers
    {

        public static OrderFailedMessage GetMessageBodyAsOrderFailedMessage(this BasicDeliverEventArgs args)
        {
            var jsonMessage = Encoding.UTF8.GetString(args.Body.ToArray());
            var orderFailedMessage = JsonSerializer.Deserialize<OrderFailedMessage>(
                jsonMessage, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return orderFailedMessage;
        }
        public static OrderApprovedMessage GetMessageBodyAsOrderApprovedMessage(this BasicDeliverEventArgs args)
        {
            var jsonMessage = Encoding.UTF8.GetString(args.Body.ToArray());
            var orderApprovedMessage = JsonSerializer.Deserialize<OrderApprovedMessage>(
                jsonMessage, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return orderApprovedMessage;
        }

        public static byte[] ConvertToMessageBodyBytes(this IOrderMessage message)
        {
            var jsonMessage = JsonSerializer.Serialize(message, message.GetType());
            var messageBody = Encoding.UTF8.GetBytes(jsonMessage);

            return messageBody;
        }

        public static T GetOrderMessageObj<T>(this OutBoxMessage outBoxMessage)
          where T : IOrderMessage
        {
            return JsonSerializer.Deserialize<T>(
                outBoxMessage.Payload,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
        }
    }
}
