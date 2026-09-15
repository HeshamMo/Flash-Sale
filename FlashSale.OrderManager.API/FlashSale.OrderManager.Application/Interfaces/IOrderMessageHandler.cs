using FlashSale.OrderManager.Application.Messaging.Messages;

namespace FlashSale.OrderManager.Application.Interfaces
{
    public interface IOrderMessageHandler
    {
        public Task HandleOrderFailed(OrderFailedMessage message);
        public Task HandleOrderApproved(OrderApprovedMessage message);


    }
}
