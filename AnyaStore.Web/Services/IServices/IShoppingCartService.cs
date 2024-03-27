using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnyaStore.Web.Models.DTO;

namespace AnyaStore.Web.Services.IServices
{
    public interface IShoppingCartService
    {
        Task<T> GetCartByUserAsync<T>(string userId);
        Task<T> ApplyCouponAsync<T>(CartHeaderDTO cartHeaderDTO);
        Task<T> RemoveCouponAsync<T>(string cartId);
        Task<T> CartUpsertAsync<T>(CartUpsertDTO cartUpsertDTO);
        Task<T> RemoveFromCartAsync<T>(CartDetailsRemoveDTO cartDetailsDTO);
    }
}