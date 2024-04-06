using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AnyaStore.Services.ShoppingCartAPI.Models.DTO;
using AnyaStore.Services.ShoppingCartAPI.Services.IServices;
using Newtonsoft.Json;

namespace AnyaStore.Services.ShoppingCartAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<ProductDTO>> GetProducts()
        {
            var client = _httpClientFactory.CreateClient("Product");
            var httpResponse = await client.GetAsync($"/api/products/");
            var apiContent = await httpResponse.Content.ReadAsStringAsync();
            var responseDTO = JsonConvert.DeserializeObject<ResponseDTO>(apiContent);

            if (httpResponse.StatusCode == HttpStatusCode.OK && responseDTO != null && responseDTO.IsSuccess)
            {
                return JsonConvert.DeserializeObject<IEnumerable<ProductDTO>>(Convert.ToString(responseDTO.Result));
            }

            return new List<ProductDTO>();
        }
    }
}