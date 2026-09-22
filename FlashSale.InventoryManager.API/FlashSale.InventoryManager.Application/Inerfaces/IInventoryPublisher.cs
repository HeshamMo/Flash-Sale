using RabbitMQ.Client;

namespace FlashSale.InventoryManager.Application.Interfaces
{
    public interface IInventoryPublisher
    {

        Task PublishOrderFail(IChannel channel, Guid orderId);
        Task publishOrderApproved(IChannel channel, Guid orderId);
    }
}
