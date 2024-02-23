using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AnyaStore.Services.ProductAPI.Migrations
{
    /// <inheritdoc />
    public partial class product_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "CategoryId", "CreatedOn", "Description", "ImageUrl", "LastUpdated", "Name", "Price" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2024, 2, 23, 17, 35, 26, 126, DateTimeKind.Local).AddTicks(615), "Ultrabook gaming", "https://placehold.co/603x403", new DateTime(2024, 2, 23, 17, 35, 26, 126, DateTimeKind.Local).AddTicks(596), "Zephyrus g16", 15.0 },
                    { 2, null, new DateTime(2024, 2, 23, 17, 35, 26, 126, DateTimeKind.Local).AddTicks(697), "Ultrabook for working", "https://placehold.co/603x403", new DateTime(2024, 2, 23, 17, 35, 26, 126, DateTimeKind.Local).AddTicks(695), "Macbook pro 16 m3", 15.0 },
                    { 3, null, new DateTime(2024, 2, 23, 17, 35, 26, 126, DateTimeKind.Local).AddTicks(744), "Ultrabook for business", "https://placehold.co/603x403", new DateTime(2024, 2, 23, 17, 35, 26, 126, DateTimeKind.Local).AddTicks(743), "Dell XPS 16", 15.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
