using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gateway.Infrastructure.Migrations
{
    public partial class SeedAdminAndCustomer:Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var adminRoleId =
                new Guid("11111111-1111-1111-1111-111111111111");

            var customerRoleId =
                new Guid("22222222-2222-2222-2222-222222222222");

            var adminUserId =
                new Guid("33333333-3333-3333-3333-333333333333");

            var customerUserId =
                new Guid("44444444-4444-4444-4444-444444444444");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[]
                {
                    "Id",
                    "Name",
                    "NormalizedName",
                    "ConcurrencyStamp"
                },
                values: new object[,]
                {
                    {
                        adminRoleId,
                        "admin",
                        "ADMIN",
                        "11111111-1111-1111-1111-111111111111"
                    },
                    {
                        customerRoleId,
                        "customer",
                        "CUSTOMER",
                        "22222222-2222-2222-2222-222222222222"
                    }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[]
                {
                    "Id",
                    "UserName",
                    "NormalizedUserName",
                    "Email",
                    "NormalizedEmail",
                    "EmailConfirmed",
                    "PasswordHash",
                    "SecurityStamp",
                    "ConcurrencyStamp",
                    "PhoneNumber",
                    "PhoneNumberConfirmed",
                    "TwoFactorEnabled",
                    "LockoutEnd",
                    "LockoutEnabled",
                    "AccessFailedCount"
                },
                values: new object[,]
                {
                    {
                        adminUserId,
                        "admin@gmail.com",
                        "ADMIN@GMAIL.COM",
                        "admin@gmail.com",
                        "ADMIN@GMAIL.COM",
                        true,

                        // Dummy password for seeding: Admin@123
                        "AQAAAAEAAYagAAAAECAHtHTUecMHLsNHHHlnkgRSk+EPeTSuFy4Ufkzzj6zDhzphWlFDxU4XknyE1PsawQ==",

                        "55555555-5555-5555-5555-555555555555",
                        "66666666-6666-6666-6666-666666666666",
                        null,
                        false,
                        false,
                        null,
                        false,
                        0
                    },
                    {
                        customerUserId,
                        "customer@gmail.com",
                        "CUSTOMER@GMAIL.COM",
                        "customer@gmail.com",
                        "CUSTOMER@GMAIL.COM",
                        true,

                        // Dummy password for seeding: Customer@123
                        "AQAAAAEAAYagAAAAEIRLOE0F17BLgAePwccvkLYW2x1rlIGaRAiQwpNyaTG/D1NtVEJkwwmTceaaEOx+jw==",

                        "77777777-7777-7777-7777-777777777777",
                        "88888888-8888-8888-8888-888888888888",
                        null,
                        false,
                        false,
                        null,
                        false,
                        0
                    }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[]
                {
                    "UserId",
                    "RoleId"
                },
                values: new object[,]
                {
                    {
                        adminUserId,
                        adminRoleId
                    },
                    {
                        customerUserId,
                        customerRoleId
                    }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var adminRoleId =
                new Guid("11111111-1111-1111-1111-111111111111");

            var customerRoleId =
                new Guid("22222222-2222-2222-2222-222222222222");

            var adminUserId =
                new Guid("33333333-3333-3333-3333-333333333333");

            var customerUserId =
                new Guid("44444444-4444-4444-4444-444444444444");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[]
                {
                    "UserId",
                    "RoleId"
                },
                keyValues: new object[]
                {
                    adminUserId,
                    adminRoleId
                });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[]
                {
                    "UserId",
                    "RoleId"
                },
                keyValues: new object[]
                {
                    customerUserId,
                    customerRoleId
                });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: adminUserId);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: customerUserId);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: adminRoleId);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: customerRoleId);
        }
    }
}