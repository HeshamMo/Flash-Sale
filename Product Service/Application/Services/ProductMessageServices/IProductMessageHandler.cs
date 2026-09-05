using ProductManager.Application.MessagingQueue.Messages;

namespace ProductManager.Application.Services.ProductMessageServices
{
    public interface IProductMessageHandler
    {
        Task HandleOrderCreated(OrderCreatedMessage message);
    }
}
