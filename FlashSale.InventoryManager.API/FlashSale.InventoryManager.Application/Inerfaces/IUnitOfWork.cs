using FlashSale.InventoryManager.Infrastructure.Persistance.Repositories;

namespace FlashSale.InventoryManager.Application.Inerfaces
{
    public interface IUnitOfWork
    {


        IInventoryRepository Inventory { get; }
        Task<int> SaveChangesAsync();
    }
}
