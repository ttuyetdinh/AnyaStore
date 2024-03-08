using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnyaStore.Services.ShoppingCartAPI.Data;
using AnyaStore.Services.ShoppingCartAPI.Models;
using AnyaStore.Services.ShoppingCartAPI.Repository.IRepository;

namespace AnyaStore.Services.ShoppingCartAPI.Repository
{
    public class CartDetailsRepository : Repository<CartDetails>, ICartDetailRepository
    {
        private readonly AppDbContext _db;
        public CartDetailsRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<CartDetails> UpdateAsync(CartDetails entity)
        {
            entity.LastUpdated = DateTime.Now;
            _db.CartDetails.Update(entity);

            await SaveAsync();

            return entity;
        }

        public override Task CreateAsync(CartDetails entity)
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