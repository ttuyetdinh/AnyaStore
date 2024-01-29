using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnyaStore.Services.CouponAPI.Data;
using AnyaStore.Services.CouponAPI.Models;
using AnyaStore.Services.CouponAPI.Repository.IRepository;

namespace AnyaStore.Services.CouponAPI.Repository
{
    public class CouponRepository : Repository<Coupon>, ICouponRepository
    {
        private readonly AppDbContext _db;
        public CouponRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Coupon> UpdateAsync(Coupon entity)
        {
            entity.LastUpdated = DateTime.Now;
            _db.Coupons.Update(entity);

            await SaveAsync();

            return entity;
        }

        public override Task CreateAsync(Coupon entity)
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