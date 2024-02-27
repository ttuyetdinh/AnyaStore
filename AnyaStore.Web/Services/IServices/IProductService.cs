using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnyaStore.Web.Models.DTO;

namespace AnyaStore.Web.Services.IServices
{
    public interface IProductService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        // doesn't need to declare in the interface
        // Task<T> GetByCategoryAsync<T>(string code);
        Task<T> CreateAsync<T>(ProductDTO dto);
        Task<T> UpdateAsync<T>(ProductDTO dto);
        Task<T> DeleteAsync<T>(int id);
    }
}