using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlashSale.InventoryManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReserveandReleaseStockProcedures:Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                CREATE TYPE dbo.OrderItemTableType AS TABLE
                (
                    ProductId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
                    ProductQuantity INT NOT NULL
                );
                """);

            migrationBuilder.Sql("""
                CREATE PROCEDURE dbo.ReserveStock
                    @OrderId     UNIQUEIDENTIFIER,
                    @CreatedAt   DATETIMEOFFSET,
                    @OrderItems  dbo.OrderItemTableType READONLY
                AS
                BEGIN
                    SET NOCOUNT ON;
                    SET XACT_ABORT ON;

                    DECLARE @ExpectedCount INT = (
                        SELECT COUNT(*)
                        FROM @OrderItems
                    );

                    DECLARE @UpdatedCount INT;

                    IF @ExpectedCount = 0
                    BEGIN
                        SELECT CAST(0 AS BIT);
                        RETURN;
                    END;

                    BEGIN TRANSACTION;

                    UPDATE p
                    SET p.Quantity = p.Quantity - oi.ProductQuantity
                    FROM dbo.Products p
                    INNER JOIN @OrderItems oi
                        ON p.Id = oi.ProductId
                    WHERE p.Quantity >= oi.ProductQuantity;

                    SET @UpdatedCount = @@ROWCOUNT;

                    IF @UpdatedCount <> @ExpectedCount
                    BEGIN
                        ROLLBACK TRANSACTION;

                        SELECT CAST(0 AS BIT);
                        RETURN;
                    END;

                    INSERT INTO dbo.InventoryTransactions
                    (
                        Id,
                        ProductId,
                        OrderId,
                        CreatedAt,
                        Quantity,
                        Description,
                        Type
                    )
                    SELECT
                        NEWID(),
                        oi.ProductId,
                        @OrderId,
                        @CreatedAt,
                        oi.ProductQuantity,
                        CONCAT('Stock reserved for order ', @OrderId),
                        'Reservation'
                    FROM @OrderItems oi;

                    COMMIT TRANSACTION;

                    SELECT CAST(1 AS BIT);
                END;
                """);

            migrationBuilder.Sql("""
                CREATE PROCEDURE dbo.ReleaseStock
                    @OrderId     UNIQUEIDENTIFIER,
                    @CreatedAt   DATETIMEOFFSET,
                    @OrderItems  dbo.OrderItemTableType READONLY
                AS
                BEGIN
                    SET NOCOUNT ON;
                    SET XACT_ABORT ON;

                    DECLARE @ExpectedCount INT = (
                        SELECT COUNT(*)
                        FROM @OrderItems
                    );

                    DECLARE @UpdatedCount INT;

                    IF @ExpectedCount = 0
                    BEGIN
                        SELECT CAST(0 AS BIT);
                        RETURN;
                    END;

                    BEGIN TRANSACTION;

                    UPDATE p
                    SET p.Quantity = p.Quantity + oi.ProductQuantity
                    FROM dbo.Products p
                    INNER JOIN @OrderItems oi
                        ON p.Id = oi.ProductId;

                    SET @UpdatedCount = @@ROWCOUNT;

                    IF @UpdatedCount <> @ExpectedCount
                    BEGIN
                        ROLLBACK TRANSACTION;

                        SELECT CAST(0 AS BIT);
                        RETURN;
                    END;

                    INSERT INTO dbo.InventoryTransactions
                    (
                        Id,
                        ProductId,
                        OrderId,
                        CreatedAt,
                        Quantity,
                        Description,
                        Type
                    )
                    SELECT
                        NEWID(),
                        oi.ProductId,
                        @OrderId,
                        @CreatedAt,
                        oi.ProductQuantity,
                        CONCAT('Stock released for order ', @OrderId),
                        'Release'
                    FROM @OrderItems oi;

                    COMMIT TRANSACTION;

                    SELECT CAST(1 AS BIT);
                END;
                """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DROP PROCEDURE dbo.ReserveStock;
                """);

            migrationBuilder.Sql("""
                DROP PROCEDURE dbo.ReleaseStock;
                """);

            migrationBuilder.Sql("""
                DROP TYPE dbo.OrderItemTableType;
                """);
        }
    }
}