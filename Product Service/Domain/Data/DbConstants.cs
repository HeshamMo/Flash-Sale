namespace ProductManager.Domain.Data
{
    public class DbConstants
    {
        public const string OrderItemsParameter = "@OrderItems";
        public const string OrderItemTableType = "dbo.OrderItemTableType";
        public const string ReserveStockProcedure = "dbo.ReserveStock";
        public const string ReserveStockCommand =
            "EXEC dbo.ReserveStock @OrderItems";

        public const string ProductIdColumn = "ProductId";
        public const string ProductQuantityColumn = "ProductQuantity";
    }
}
