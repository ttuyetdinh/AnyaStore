using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnyaStore.Services.ShoppingCartAPI.Data;
using AnyaStore.Services.ShoppingCartAPI.Models;
using AnyaStore.Services.ShoppingCartAPI.Repository.IRepository;

namespace AnyaStore.Services.ShoppingCartAPI.Repository
{
    public class CartHeaderRepository : Repository<CartHeader>, ICartHeaderRepository
    {
        private readonly AppDbContext _db;
        public CartHeaderRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<CartHeader> UpdateAsync(CartHeader entity)
        {
            entity.LastUpdated = DateTime.Now;
            _db.CartHeaders.Update(entity);

            await SaveAsync();

            return entity;
        }

        public override Task CreateAsync(CartHeader entity)
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