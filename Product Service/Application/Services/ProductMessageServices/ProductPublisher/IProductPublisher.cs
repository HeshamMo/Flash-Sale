using RabbitMQ.Client;

namespace ProductManager.Application.Services.ProductMessageServices.ProductPublisher
{
    public interface IProductPublisher
    {

        Task PublishOrderFail(IChannel channel, Guid orderId);
        Task publishOrderApproved(IChannel channel, Guid orderId);
        byte[] ConvertObjectToMQBody(object obj);
    }
}
