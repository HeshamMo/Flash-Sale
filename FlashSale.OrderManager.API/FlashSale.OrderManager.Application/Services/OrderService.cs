using AutoMapper;
using FlashSale.OrderManager.Application.DTOs.Orders;
using FlashSale.OrderManager.Application.Interfaces;
using FlashSale.OrderManager.Domain.Enums;
using FlashSale.OrderManager.Domain.Models;

namespace FlashSale.OrderManager.Application.Services
{
    public class OrderService:IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;
        private readonly TimeProvider _timeProvider;


        public OrderService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ICurrentUser currentUser,
            TimeProvider timeProvider)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUser = currentUser;
            _timeProvider = timeProvider;
        }



        public async Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request)
        {

            var order = _mapper.Map<Order>(request);

            order.Id = Guid.NewGuid();
            order.CustomerId = _currentUser.GetUserId();
            order.Status = OrderStatus.Pending;
            order.CreatedAtUtc = _timeProvider.GetUtcNow();

            await _unitOfWork.Orders.AddAsync(order);


            var outboxMessage = _unitOfWork.Outbox.CreateOrderCreatedOutBoxMessage(order, _timeProvider.GetUtcNow());

            await _unitOfWork.Outbox.AddAsync(outboxMessage);

            // Commit Order + OutboxMessage together
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<OrderResponse>(order);
        }

        public async Task<OrderResponse> GetOrderByIdAsync(Guid orderId)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
            return _mapper.Map<OrderResponse>(order);
        }

        public async Task<IEnumerable<OrderResponse>> GetOrdersByUserIdAsync()
        {
            var order = await _unitOfWork.Orders.GetOrdersByUserId(_currentUser.GetUserId());
            return _mapper.Map<IEnumerable<OrderResponse>>(order);
        }

        public Task<OrderResponse> CancelOrderAsync(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public Task<OrderResponse> CompleteOrderAsync(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<OrderResponse>> GetAllOrders()
        {
            return _mapper.Map<IEnumerable<OrderResponse>>(await _unitOfWork.Orders.GetAllOrders());
        }
    }
}