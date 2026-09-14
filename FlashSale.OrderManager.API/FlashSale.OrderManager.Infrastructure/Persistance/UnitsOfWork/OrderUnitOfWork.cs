using FlashSale.OrderManager.Application.Interfaces;

namespace FlashSale.OrderManager.Infrastructure.Persistance.UnitsOfWork;
public class UnitOfWork:IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public IOrderRepository Orders { get; }

    public IOutboxRepository Outbox { get; }

    public UnitOfWork(
        ApplicationDbContext context,
        IOrderRepository orderRepository,
        IOutboxRepository outboxRepository)
    {
        _context = context;
        Orders = orderRepository;
        Outbox = outboxRepository;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}