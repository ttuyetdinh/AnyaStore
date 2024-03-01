using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AnyaStore.Services.ProductAPI.Migrations
{
    /// <inheritdoc />
    public partial class category_seed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CreatedOn", "LastUpdated", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 2, 29, 13, 17, 8, 174, DateTimeKind.Local).AddTicks(6601), new DateTime(2024, 2, 29, 13, 17, 8, 174, DateTimeKind.Local).AddTicks(6600), "Bussiness Laptop" },
                    { 2, new DateTime(2024, 2, 29, 13, 17, 8, 174, DateTimeKind.Local).AddTicks(6616), new DateTime(2024, 2, 29, 13, 17, 8, 174, DateTimeKind.Local).AddTicks(6615), "Gaming Laptop" },
                    { 3, new DateTime(2024, 2, 29, 13, 17, 8, 174, DateTimeKind.Local).AddTicks(6627), new DateTime(2024, 2, 29, 13, 17, 8, 174, DateTimeKind.Local).AddTicks(6627), "Working Laptop" }
                });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                columns: new[] { "CategoryId", "CreatedOn", "LastUpdated" },
                values: new object[] { 1, new DateTime(2024, 2, 29, 13, 17, 8, 174, DateTimeKind.Local).AddTicks(6533), new DateTime(2024, 2, 29, 13, 17, 8, 174, DateTimeKind.Local).AddTicks(6518) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                columns: new[] { "CategoryId", "CreatedOn", "LastUpdated" },
                values: new object[] { 2, new DateTime(2024, 2, 29, 13, 17, 8, 174, DateTimeKind.Local).AddTicks(6572), new DateTime(2024, 2, 29, 13, 17, 8, 174, DateTimeKind.Local).AddTicks(6571) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3,
                columns: new[] { "CategoryId", "CreatedOn", "LastUpdated" },
                values: new object[] { 3, new DateTime(2024, 2, 29, 13, 17, 8, 174, DateTimeKind.Local).AddTicks(6585), new DateTime(2024, 2, 29, 13, 17, 8, 174, DateTimeKind.Local).AddTicks(6585) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                columns: new[] { "CategoryId", "CreatedOn", "LastUpdated" },
                values: new object[] { null, new DateTime(2024, 2, 29, 13, 9, 33, 431, DateTimeKind.Local).AddTicks(2271), new DateTime(2024, 2, 29, 13, 9, 33, 431, DateTimeKind.Local).AddTicks(2252) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                columns: new[] { "CategoryId", "CreatedOn", "LastUpdated" },
                values: new object[] { null, new DateTime(2024, 2, 29, 13, 9, 33, 431, DateTimeKind.Local).AddTicks(2337), new DateTime(2024, 2, 29, 13, 9, 33, 431, DateTimeKind.Local).AddTicks(2336) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3,
                columns: new[] { "CategoryId", "CreatedOn", "LastUpdated" },
                values: new object[] { null, new DateTime(2024, 2, 29, 13, 9, 33, 431, DateTimeKind.Local).AddTicks(2353), new DateTime(2024, 2, 29, 13, 9, 33, 431, DateTimeKind.Local).AddTicks(2352) });
        }
    }
}
