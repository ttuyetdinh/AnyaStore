using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnyaStore.Web.Models.DTO;

namespace AnyaStore.Web.Services.IServices
{
    public interface ICouponService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> GetByCodeAsync<T>(string code);
        Task<T> CreateAsync<T>(CouponDTO dto);
        Task<T> UpdateAsync<T>(CouponDTO dto);
        Task<T> DeleteAsync<T>(int id);
    }
}