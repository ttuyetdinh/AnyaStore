using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnyaStore.Web.Models.DTO;
using AnyaStore.Web.Services.IServices;
using AnyaStore.Web.Ultilities;

namespace AnyaStore.Web.Services
{
    public class CategoryService : ICategoryService
    {
        // private readonly IHttpClientFactory _clientFactory;
        private readonly IBaseService _baseService;
        private readonly string productUrl;
        public CategoryService(IBaseService baseService, IConfiguration configuration)
        {
            // _clientFactory = clientFactory;
            _baseService = baseService;
            productUrl = configuration.GetValue<string>("ServiceUrls:ProductApi");
            _baseService.SetBaseUrl(productUrl);
        }

        public async Task<T> CreateAsync<T>(CategoryDTO dto)
        {
            return await _baseService.SendAsync<T>(new RequestDTO()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = $"{productUrl}/api/CategoryAPI",
            });
        }

        public async Task<T> DeleteAsync<T>(int id)
        {
            return await _baseService.SendAsync<T>(new RequestDTO()
            {
                ApiType = SD.ApiType.DELETE,
                Url = $"{productUrl}/api/CategoryAPI/{id}",
            });
        }

        public async Task<T> GetAllAsync<T>()
        {
            return await _baseService.SendAsync<T>(new RequestDTO()
            {
                ApiType = SD.ApiType.GET,
                Url = $"{productUrl}/api/CategoryAPI",
            });
        }

        public async Task<T> GetAsync<T>(int id)
        {
            return await _baseService.SendAsync<T>(new RequestDTO()
            {
                ApiType = SD.ApiType.GET,
                Url = $"{productUrl}/api/CategoryAPI/{id}",
            });
        }

        public async Task<T> UpdateAsync<T>(CategoryDTO dto)
        {
            return await _baseService.SendAsync<T>(new RequestDTO()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = $"{productUrl}/api/CategoryAPI",
            });
        }
    }
}