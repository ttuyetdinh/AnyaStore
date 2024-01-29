using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnyaStore.Services.CouponAPI.Migrations
{
    /// <inheritdoc />
    public partial class Coupon_seed_data_auto_time : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "CouponId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "LastUpdated" },
                values: new object[] { new DateTime(2024, 1, 29, 14, 56, 1, 91, DateTimeKind.Local).AddTicks(7392), new DateTime(2024, 1, 29, 14, 56, 1, 91, DateTimeKind.Local).AddTicks(7377) });

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "CouponId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "LastUpdated" },
                values: new object[] { new DateTime(2024, 1, 29, 14, 56, 1, 91, DateTimeKind.Local).AddTicks(7431), new DateTime(2024, 1, 29, 14, 56, 1, 91, DateTimeKind.Local).AddTicks(7430) });

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "CouponId",
                keyValue: 3,
                columns: new[] { "CreatedOn", "LastUpdated" },
                values: new object[] { new DateTime(2024, 1, 29, 14, 56, 1, 91, DateTimeKind.Local).AddTicks(7443), new DateTime(2024, 1, 29, 14, 56, 1, 91, DateTimeKind.Local).AddTicks(7442) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "CouponId",
                keyValue: 1,
                columns: new[] { "CreatedOn", "LastUpdated" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "CouponId",
                keyValue: 2,
                columns: new[] { "CreatedOn", "LastUpdated" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "CouponId",
                keyValue: 3,
                columns: new[] { "CreatedOn", "LastUpdated" },
                values: new object[] { null, null });
        }
    }
}
