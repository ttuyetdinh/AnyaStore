using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnyaStore.Services.CouponAPI.Models;

namespace AnyaStore.Services.CouponAPI.Repository.IRepository
{
    public interface ICouponRepository : IRepository<Coupon>
    {
        Task<Coupon> UpdateAsync(Coupon entity);
    }
}