
using FlashSale.InventoryManager.Application.Inerfaces;
using FlashSale.InventoryManager.Application.Interfaces;
using FlashSale.InventoryManager.Application.Messaging.Messages.OrderCreated;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace FlashSale.InventoryManager.Infrastructure.Messaging.Consumers.MessageHandlers
{
    public class OrderMessageHandler:IOrderMessageHandler
    {

        private readonly IConnection _connection;
        private readonly IInventoryService _productService;
        private readonly IInventoryPublisher _productPublisher;
        public OrderMessageHandler(IConnection connection, IInventoryService productService, IInventoryPublisher productPublisher)
        {
            _connection = connection;

            _productService = productService;
            _productPublisher = productPublisher;
        }

        public async Task HandleOrderCreated(OrderCreatedMessage message)
        {
            using var channel = await _connection.CreateChannelAsync();

            if(message is null || message.Products.Any() is false)
            {
                await _productPublisher.PublishOrderFail(channel, message.OrderId);
            }


            var ReseverStockSuccess = await _productService.ReserveStockAsync(message.OrderId, message.Products);

            if(ReseverStockSuccess is true)
            {
                await _productPublisher.publishOrderApproved(channel, message.OrderId);
            }

            else if(ReseverStockSuccess is false)
            {
                await _productPublisher.PublishOrderFail(channel, message.OrderId);
            }


        }



        public byte[] ConvertObjectToMQBody(object obj)
        {
            return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj));
        }
    }
}
