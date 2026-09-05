using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductManager.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddStockReservation:Migration
    {
        /// <inheritdoc />
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
            @OrderItems dbo.OrderItemTableType READONLY
        AS
        BEGIN
            SET NOCOUNT ON;
            SET XACT_ABORT ON;

            BEGIN TRANSACTION;

            UPDATE p
            SET p.Quantity = p.Quantity - oi.ProductQuantity
            FROM dbo.Products p
            INNER JOIN @OrderItems oi
                ON p.Id = oi.ProductId
            WHERE p.Quantity >= oi.ProductQuantity;

            IF @@ROWCOUNT <> (SELECT COUNT(*) FROM @OrderItems)
            BEGIN
                ROLLBACK;
                SELECT CAST(0 AS BIT);
                RETURN;
            END;

            COMMIT;

            SELECT CAST(1 AS BIT);
        END;
        """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
        DROP PROCEDURE IF EXISTS dbo.ReserveStock;
        """);

            migrationBuilder.Sql("""
        DROP TYPE IF EXISTS dbo.OrderItemTableType;
        """);
        }
    }
}
