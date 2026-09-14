namespace FlashSale.OrderManager.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IOrderRepository Orders { get; }

        IOutboxRepository Outbox { get; }

        Task<int> SaveChangesAsync();
    }
}
