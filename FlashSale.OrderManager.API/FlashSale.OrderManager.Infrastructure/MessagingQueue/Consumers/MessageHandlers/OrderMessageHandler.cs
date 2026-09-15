using FlashSale.OrderManager.Application.Interfaces;
using FlashSale.OrderManager.Application.Messaging.Messages;
using FlashSale.OrderManager.Domain.Enums;

namespace FlashSale.OrderManager.Infrastructure.MessagingQueue.Consumers.MessageHandlers
{
    public class OrderMessageHandler:IOrderMessageHandler
    {

        private readonly IUnitOfWork _unitOfWork;

        public OrderMessageHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task HandleOrderFailed(OrderFailedMessage message)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(message.OrderId);
            order.Status = OrderStatus.StockFailed;

            var updateResult = await _unitOfWork.SaveChangesAsync();

        }

        public async Task HandleOrderApproved(OrderApprovedMessage message)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(message.OrderId);
            order.Status = OrderStatus.StockReserved;

            var updateResult = await _unitOfWork.SaveChangesAsync();
        }
    }
}
