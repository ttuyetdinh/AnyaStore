using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnyaStore.Services.ProductAPI.Data;
using AnyaStore.Services.ProductAPI.Models;
using AnyaStore.Services.ProductAPI.Repository.IRepository;

namespace AnyaStore.Services.ProductAPI.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly AppDbContext _db;
        public CategoryRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Category> UpdateAsync(Category entity)
        {
            entity.LastUpdated = DateTime.Now;
            _db.Categories.Update(entity);

            await SaveAsync();

            return entity;
        }

        public override Task CreateAsync(Category entity)
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