using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnyaStore.Services.ProductAPI.Models;

// using AnyaStore.Services.ProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AnyaStore.Services.ProductAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }

        // create a seed method to add data to the database
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 1,
                Name = "Zephyrus g16",
                Price = 15,
                Description = "Ultrabook gaming",
                ImageUrl = "https://placehold.co/603x403",
                // CategoryId = 1
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 2,
                Name = "Macbook pro 16 m3",
                Price = 15,
                Description = "Ultrabook for working",
                ImageUrl = "https://placehold.co/603x403",
                // CategoryId = 2
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 3,
                Name = "Dell XPS 16",
                Price = 15,
                Description = "Ultrabook for business",
                ImageUrl = "https://placehold.co/603x403",
                // CategoryId = 2
            });
        }
    }
}