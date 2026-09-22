using FlashSale.OrderManager.Application.Interfaces;
using FlashSale.OrderManager.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FlashSale.OrderManager.Infrastructure.Persistance.Repositories
{
    public class OrderRepository:IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(Guid orderId)
        {
            return await _context.Orders
                .Include(x => x.Products)
                .FirstOrDefaultAsync(x => x.Id == orderId);
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserId(Guid userId)
        {
            return await _context.Orders
                .Where(x => x.CustomerId == userId)
                .Include(x => x.Products)
                .AsSplitQuery()
                .ToListAsync();
        }

        public Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);

            return Task.CompletedTask;
        }
    }
}