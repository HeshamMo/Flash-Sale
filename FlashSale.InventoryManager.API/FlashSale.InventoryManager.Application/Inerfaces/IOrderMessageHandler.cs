
using FlashSale.InventoryManager.Application.Messaging.Messages.OrderCreated;

namespace FlashSale.InventoryManager.Application.Inerfaces
{
    public interface IOrderMessageHandler
    {
        Task HandleOrderCreated(OrderCreatedMessage message);
    }
}
