using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnyaStore.Web.Models.DTO;
using AnyaStore.Web.Services.IServices;
using AnyaStore.Web.Ultilities;

namespace AnyaStore.Web.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        // private readonly IHttpClientFactory _clientFactory;
        private readonly IBaseService _baseService;
        private readonly string shoppingCartUrl;
        public ShoppingCartService(IBaseService baseService, IConfiguration configuration)
        {
            // _clientFactory = clientFactory;
            _baseService = baseService;
            shoppingCartUrl = configuration.GetValue<string>("ServiceUrls:ShoppingCartApi");
            _baseService.SetBaseUrl(shoppingCartUrl);
        }

        public async Task<T> ApplyCouponAsync<T>(int cartId, CartHeaderDTO cartHeaderDTO)
        {
            return await _baseService.SendAsync<T>(new RequestDTO()
            {
                ApiType = SD.ApiType.POST,
                Data = cartHeaderDTO,
                Url = $"{shoppingCartUrl}/api/carts/{cartId}/applycoupon",
            });
        }

        public async Task<T> CartUpsertAsync<T>(CartUpsertDTO cartUpsertDTO)
        {
            return await _baseService.SendAsync<T>(new RequestDTO()
            {
                ApiType = SD.ApiType.POST,
                Data = cartUpsertDTO,
                Url = $"{shoppingCartUrl}/api/carts/cartupsert",
            });
        }

        public async Task<T> GetCartByUserAsync<T>(string userId)
        {
            return await _baseService.SendAsync<T>(new RequestDTO()
            {
                ApiType = SD.ApiType.GET,
                Url = $"{shoppingCartUrl}/api/carts/users/{userId}",
            });
        }

        public async Task<T> RemoveCouponAsync<T>(string cartId)
        {
            return await _baseService.SendAsync<T>(new RequestDTO()
            {
                ApiType = SD.ApiType.POST,
                Url = $"{shoppingCartUrl}/api/carts/{cartId}/RemoveCoupon",
            });
        }

        public async Task<T> RemoveFromCartAsync<T>(int cartId, int itemId)
        {
            return await _baseService.SendAsync<T>(new RequestDTO()
            {
                ApiType = SD.ApiType.POST,
                Data = null,
                Url = $"{shoppingCartUrl}/api/carts/{cartId}/item/{itemId}",
            });
        }
    }
}