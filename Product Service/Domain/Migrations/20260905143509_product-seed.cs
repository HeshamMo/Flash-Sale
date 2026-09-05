using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductManager.Domain.Migrations
{
    /// <inheritdoc />
    public partial class productseed:Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[]
                {
                    "Id",
                    "Name",
                    "Description",
                    "Price",
                    "Quantity"
                },
                values: new object[,]
                {
                    {
                        Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        "Laptop",
                        "High performance laptop",
                        45000m,
                        10
                    },
                    {
                        Guid.Parse("22222222-2222-2222-2222-222222222222"),
                        "Mouse",
                        "Wireless mouse",
                        750m,
                        50
                    },
                    {
                        Guid.Parse("33333333-3333-3333-3333-333333333333"),
                        "Keyboard",
                        "Mechanical keyboard",
                        2500m,
                        30
                    },
                    {
                        Guid.Parse("44444444-4444-4444-4444-444444444444"),
                        "Monitor",
                        "24 inch Full HD monitor",
                        7000m,
                        20
                    },
                    {
                        Guid.Parse("55555555-5555-5555-5555-555555555555"),
                        "Headphones",
                        "Wireless headphones",
                        3000m,
                        15
                    }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValues: new object[]
                {
                    Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Guid.Parse("55555555-5555-5555-5555-555555555555")
                });
        }
    }
}