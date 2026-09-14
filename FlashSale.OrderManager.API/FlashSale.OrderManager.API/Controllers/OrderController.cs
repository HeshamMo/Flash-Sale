using FlashSale.OrderManager.Application.DTOs.Orders;
using FlashSale.OrderManager.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FlashSale.OrderManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController:ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("GetAllOrders")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrders();

            return Ok(orders);
        }
        [HttpGet]
        public async Task<IActionResult> GetOrdersByUserId()
        {
            var orders = await _orderService.GetOrdersByUserIdAsync();

            return Ok(orders);
        }

        [HttpGet("{orderId:guid}")]
        public async Task<IActionResult> GetOrderById(Guid orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);

            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(
            [FromBody] CreateOrderRequest request)
        {
            var order = await _orderService.CreateOrderAsync(request);



            return CreatedAtAction(
                nameof(GetOrderById),
                new { orderId = order.Id },
                order);
        }

        [HttpPost("{orderId:guid}/cancel")]
        public async Task<IActionResult> CancelOrder(Guid orderId)
        {
            var order = await _orderService.CancelOrderAsync(orderId);

            return Ok(order);
        }

        [HttpPost("{orderId:guid}/complete")]
        public async Task<IActionResult> CompleteOrder(Guid orderId)
        {
            var order = await _orderService.CompleteOrderAsync(orderId);

            return Ok(order);
        }
    }
}