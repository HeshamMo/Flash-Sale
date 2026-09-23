using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlashSale.InventoryManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedProducts:Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Name", "Description", "Price", "Quantity" },
                values: new object[,]
                {
                    {
                        new Guid("00000000-0000-0000-0000-000000000001"),
                        "Intel Core i9-14900K",
                        "24 Cores (8P+16E) / 32 Threads, 3.2GHz Base / 6.0GHz Turbo, 36MB Smart Cache, LGA1700 Socket, 125W TDP",
                        589.99m,
                        50
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000002"),
                        "AMD Ryzen 9 7950X3D",
                        "16 Cores / 32 Threads, 4.2GHz Base / 5.7GHz Boost, 128MB 3D V-Cache, AM5 Socket, 120W TDP",
                        599.00m,
                        40
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000003"),
                        "NVIDIA GeForce RTX 4090",
                        "24GB GDDR6X, 16384 CUDA Cores, 2.52GHz Boost Clock, PCIe 4.0, 450W TGP",
                        1599.99m,
                        25
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000004"),
                        "AMD Radeon RX 7900 XTX",
                        "24GB GDDR6, 6144 Stream Processors, 2.5GHz Game Clock, PCIe 4.0, 355W TBP",
                        949.99m,
                        30
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000005"),
                        "Corsair Vengeance DDR5 32GB Kit",
                        "2x16GB, DDR5-6000, CL30, 1.35V, XMP 3.0 Support",
                        129.99m,
                        100
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000006"),
                        "Samsung 990 PRO 2TB NVMe SSD",
                        "PCIe 4.0 x4, 7450MB/s Read / 6900MB/s Write, V-NAND 3-bit MLC, 1200 TBW",
                        169.99m,
                        80
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000007"),
                        "ASUS ROG Strix Z790-E Gaming WiFi",
                        "LGA1700 Socket, DDR5 Support, PCIe 5.0, WiFi 6E, 20+1 Power Stages, ATX Form Factor",
                        449.99m,
                        35
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000008"),
                        "Corsair RM1000x PSU",
                        "1000W, 80+ Gold Certified, Fully Modular, 105C Japanese Capacitors, Zero RPM Fan Mode",
                        189.99m,
                        60
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000009"),
                        "NZXT H9 Flow",
                        "Dual-Chamber Mid Tower ATX Case, High-Airflow Mesh Front, Supports up to 8 Fans, Tempered Glass Side Panel",
                        139.99m,
                        45
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000010"),
                        "Noctua NH-D15 chromax.black",
                        "Dual-Tower CPU Air Cooler, Dual 140mm NF-A15 PWM Fans, 165mm Height, Supports 220W+ TDP CPUs",
                        109.99m,
                        55
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
                    new Guid("00000000-0000-0000-0000-000000000001"),
                    new Guid("00000000-0000-0000-0000-000000000002"),
                    new Guid("00000000-0000-0000-0000-000000000003"),
                    new Guid("00000000-0000-0000-0000-000000000004"),
                    new Guid("00000000-0000-0000-0000-000000000005"),
                    new Guid("00000000-0000-0000-0000-000000000006"),
                    new Guid("00000000-0000-0000-0000-000000000007"),
                    new Guid("00000000-0000-0000-0000-000000000008"),
                    new Guid("00000000-0000-0000-0000-000000000009"),
                    new Guid("00000000-0000-0000-0000-000000000010")
                });
        }
    }
}