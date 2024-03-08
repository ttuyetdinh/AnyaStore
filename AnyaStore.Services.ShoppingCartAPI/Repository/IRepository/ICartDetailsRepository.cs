using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnyaStore.Services.ShoppingCartAPI.Models;

namespace AnyaStore.Services.ShoppingCartAPI.Repository.IRepository
{
    public interface ICartDetailRepository : IRepository<CartDetails>
    {
        Task<CartDetails> UpdateAsync(CartDetails entity);
    }
}