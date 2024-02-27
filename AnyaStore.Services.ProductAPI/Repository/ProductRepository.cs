using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnyaStore.Services.ProductAPI.Data;
using AnyaStore.Services.ProductAPI.Models;
using AnyaStore.Services.ProductAPI.Repository.IRepository;

namespace AnyaStore.Services.ProductAPI.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly AppDbContext _db;
        public ProductRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Product> UpdateAsync(Product entity)
        {
            entity.LastUpdated = DateTime.Now;
            _db.Products.Update(entity);

            await SaveAsync();

            return entity;
        }

        public override Task CreateAsync(Product entity)
        {
            if (entity.CreatedOn == default)
            {
                entity.CreatedOn = DateTime.Now;
                entity.LastUpdated = DateTime.Now;
            }
            return base.CreateAsync(entity);
        }
    }
}