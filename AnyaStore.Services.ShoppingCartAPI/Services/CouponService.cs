using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AnyaStore.Services.ShoppingCartAPI.Models.DTO;
using AnyaStore.Services.ShoppingCartAPI.Services.IServices;
using Newtonsoft.Json;

namespace AnyaStore.Services.ShoppingCartAPI.Services
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CouponService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<CouponDTO> GetCoupon(string couponCode)
        {
            var client = _httpClientFactory.CreateClient("Coupon");
            var httpResponse = await client.GetAsync($"/api/CouponAPI/GetByCode/{couponCode}");
            var apiContent = await httpResponse.Content.ReadAsStringAsync();
            var responseDTO = JsonConvert.DeserializeObject<ResponseDTO>(apiContent);

            if (httpResponse.StatusCode == HttpStatusCode.OK && responseDTO != null && responseDTO.IsSuccess)
            {
                return JsonConvert.DeserializeObject<CouponDTO>(Convert.ToString(responseDTO.Result));
            }

            return new CouponDTO();
        }
    }
}