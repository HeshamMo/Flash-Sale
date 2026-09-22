namespace FlashSale.InventoryManager.Infrastructure.Persistance
{
    public class DbQueryOptions
    {
        public const string OrderIdParameter = "@OrderId";
        public const string TransactionTypeParameter = "@TransactionType";
        public const string CreatedAtParameter = "@CreatedAt";
        public const string OrderItemsParameter = "@OrderItems";

        public const string OrderItemTableType = "dbo.OrderItemTableType";


        public const string ReserveStockCommand =
         """
            EXEC dbo.ReserveStock
                @OrderId,
                @CreatedAt,
                @OrderItems
            """;


        public const string ReleaseStockCommand =
            """
            EXEC dbo.ReleaseStock
                @OrderId,
                @CreatedAt,
                @OrderItems
            """;

        public const string ProductIdColumn = "ProductId";
        public const string ProductQuantityColumn = "ProductQuantity";
    }
}
