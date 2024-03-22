using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnyaStore.Services.ShoppingCartAPI.Migrations
{
    /// <inheritdoc />
    public partial class cartDetails_order_field_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "CartDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "CartDetails");
        }
    }
}
