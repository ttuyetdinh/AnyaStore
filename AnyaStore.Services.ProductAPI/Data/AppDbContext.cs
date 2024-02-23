using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// using AnyaStore.Services.ProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AnyaStore.Services.ProductAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        // public DbSet<Coupon> Coupons { get; set; }

        // // create a seed method to add data to the database
        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     base.OnModelCreating(modelBuilder);
        //     modelBuilder.Entity<Coupon>().HasData(new Coupon
        //     {
        //         CouponId = 1,
        //         CouponCode = "10OFF",
        //         DiscountAmount = 10,
        //         MinAmount = 100,
        //     });
        //     modelBuilder.Entity<Coupon>().HasData(new Coupon
        //     {
        //         CouponId = 2,
        //         CouponCode = "20OFF",
        //         DiscountAmount = 20,
        //         MinAmount = 200,
        //     });
        //     modelBuilder.Entity<Coupon>().HasData(new Coupon
        //     {
        //         CouponId = 3,
        //         CouponCode = "30OFF",
        //         DiscountAmount = 30,
        //         MinAmount = 300,
        //     });
        // }
    }
}