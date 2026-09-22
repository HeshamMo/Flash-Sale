using FlashSale.InventoryManager.Domain.Enums;

namespace FlashSale.InventoryManager.Domain.Models
{
    public class InventoryTransaction
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid OrderId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public Product product { get; set; }
        public InventoryTransactionType Type { get; set; }

    }
}
