using ProductManager.Application.MessagingQueue.Messages;
using ProductManager.Application.Services.ProductMessageServices.ProductPublisher;
using ProductManager.Application.Services.ProductService;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace ProductManager.Application.Services.ProductMessageServices
{
    public class ProductMessageHandler:IProductMessageHandler
    {

        private readonly IConnection _connection;
        private readonly IProductService _productService;
        private readonly IProductPublisher _productPublisher;
        public ProductMessageHandler(IConnection connection, IProductService productService, IProductPublisher productPublisher)
        {
            _connection = connection;

            _productService = productService;
            _productPublisher = productPublisher;
        }

        public async Task HandleOrderCreated(OrderCreatedMessage message)
        {
            using var channel = await _connection.CreateChannelAsync();

            if(message is null || message.OrderProducts.Any() is false)
            {
                await _productPublisher.PublishOrderFail(channel, message.OrderId);
            }


            var ReseverStockSuccess = await _productService.ReserveStockAsync(message.OrderProducts);

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
