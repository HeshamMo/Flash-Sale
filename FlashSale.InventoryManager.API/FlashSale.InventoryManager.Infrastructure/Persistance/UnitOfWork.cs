using FlashSale.InventoryManager.Application.Inerfaces;
using FlashSale.InventoryManager.Infrastructure.Persistance.Repositories;

namespace FlashSale.InventoryManager.Infrastructure.Persistance
{
    public class UnitOfWork:IUnitOfWork
    {
        public IInventoryRepository Inventory { get; }

        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context, IInventoryRepository inventoryRepository)
        {
            Inventory = inventoryRepository;
            _context = context;
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
