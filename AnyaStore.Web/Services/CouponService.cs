using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnyaStore.Web.Models.DTO;
using AnyaStore.Web.Services.IServices;
using AnyaStore.Web.Ultilities;

namespace AnyaStore.Web.Services
{
    public class CouponService : ICouponService
    {
        // private readonly IHttpClientFactory _clientFactory;
        private readonly IBaseService _baseService;
        private readonly string couponUrl;
        public CouponService(IHttpClientFactory clientFactory, IBaseService baseService, IConfiguration configuration)
        {
            // _clientFactory = clientFactory;
            _baseService = baseService;
            couponUrl = configuration.GetValue<string>("ServiceUrls:CouponApi");
            _baseService.SetBaseUrl(couponUrl);
        }


        public async Task<T> CreateAsync<T>(CouponDTO dto)
        {
            return await _baseService.SendAsync<T>(new RequestDTO()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = $"{couponUrl}/api/CouponAPI",
            });
        }

        public async Task<T> DeleteAsync<T>(int id)
        {
            return await _baseService.SendAsync<T>(new RequestDTO()
            {
                ApiType = SD.ApiType.DELETE,
                Url = $"{couponUrl}/api/CouponAPI/{id}",
            });
        }

        public async Task<T> GetAllAsync<T>()
        {
            return await _baseService.SendAsync<T>(new RequestDTO()
            {
                ApiType = SD.ApiType.GET,
                Url = $"{couponUrl}/api/CouponAPI",
            });
        }

        public async Task<T> GetAsync<T>(int id)
        {
            return await _baseService.SendAsync<T>(new RequestDTO()
            {
                ApiType = SD.ApiType.GET,
                Url = $"{couponUrl}/api/CouponAPI/{id}",
            });
        }

        public async Task<T> GetByCodeAsync<T>(string code)
        {
            return await _baseService.SendAsync<T>(new RequestDTO()
            {
                ApiType = SD.ApiType.GET,
                Url = $"{couponUrl}/api/CouponAPI/GetByCode/{code}",
            });
        }

        public async Task<T> UpdateAsync<T>(CouponDTO dto)
        {
            return await _baseService.SendAsync<T>(new RequestDTO()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = $"{couponUrl}/api/CouponAPI",
            });
        }
    }
}