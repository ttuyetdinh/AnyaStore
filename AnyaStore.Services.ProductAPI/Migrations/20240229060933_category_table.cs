using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnyaStore.Services.ProductAPI.Migrations
{
    /// <inheritdoc />
    public partial class category_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "LastUpdated" },
                values: new object[] { new DateTime(2024, 2, 29, 13, 9, 33, 431, DateTimeKind.Local).AddTicks(2271), new DateTime(2024, 2, 29, 13, 9, 33, 431, DateTimeKind.Local).AddTicks(2252) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "LastUpdated" },
                values: new object[] { new DateTime(2024, 2, 29, 13, 9, 33, 431, DateTimeKind.Local).AddTicks(2337), new DateTime(2024, 2, 29, 13, 9, 33, 431, DateTimeKind.Local).AddTicks(2336) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3,
                columns: new[] { "CreatedOn", "LastUpdated" },
                values: new object[] { new DateTime(2024, 2, 29, 13, 9, 33, 431, DateTimeKind.Local).AddTicks(2353), new DateTime(2024, 2, 29, 13, 9, 33, 431, DateTimeKind.Local).AddTicks(2352) });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Products_CategoryId",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "LastUpdated" },
                values: new object[] { new DateTime(2024, 2, 23, 17, 35, 26, 126, DateTimeKind.Local).AddTicks(615), new DateTime(2024, 2, 23, 17, 35, 26, 126, DateTimeKind.Local).AddTicks(596) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "LastUpdated" },
                values: new object[] { new DateTime(2024, 2, 23, 17, 35, 26, 126, DateTimeKind.Local).AddTicks(697), new DateTime(2024, 2, 23, 17, 35, 26, 126, DateTimeKind.Local).AddTicks(695) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3,
                columns: new[] { "CreatedOn", "LastUpdated" },
                values: new object[] { new DateTime(2024, 2, 23, 17, 35, 26, 126, DateTimeKind.Local).AddTicks(744), new DateTime(2024, 2, 23, 17, 35, 26, 126, DateTimeKind.Local).AddTicks(743) });
        }
    }
}
