using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AnyaStore.Web.Models.DTO;
using AnyaStore.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;

namespace AnyaStore.Web.Services
{
    public class BaseService : IBaseService
    {
        public IHttpClientFactory _httpClientFactory { get; set; }
        public IApiMessageRequestBuilder _apiMessageRequestBuilder { get; set; }
        public string BaseUrl { get; private set; }

        public BaseService(IHttpClientFactory httpClientFactory, IApiMessageRequestBuilder apiMessageRequestBuilder, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _apiMessageRequestBuilder = apiMessageRequestBuilder;
        }
        public void SetBaseUrl(string baseUrl)
        {
            BaseUrl = baseUrl ?? "";
        }
        public async Task<T> SendAsync<T>(RequestDTO requestDTO)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("AnyaStoreAPI");

                var message = _apiMessageRequestBuilder.Build(requestDTO);

                var response = await client.SendAsync(message);

                ResponseDTO finalResponseDTO = new()
                {
                    IsSuccess = false
                };
                try
                {
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.NotFound:
                            finalResponseDTO.ErrorMessage = new List<string>() { "Not found" };
                            break;
                        case HttpStatusCode.Forbidden:
                            finalResponseDTO.ErrorMessage = new List<string>() { "Access denied" };
                            break;
                        case HttpStatusCode.Unauthorized:
                            finalResponseDTO.ErrorMessage = new List<string>() { "Unauthorized" };
                            break;
                        case HttpStatusCode.InternalServerError:
                            finalResponseDTO.ErrorMessage = new List<string>() { "Internal server error" };
                            break;
                        case HttpStatusCode.BadRequest:
                            var error = await response.Content.ReadAsStringAsync();
                            finalResponseDTO.ErrorMessage = new List<string> { JsonConvert.DeserializeObject<ResponseDTO>(error).ToString() };
                            break;
                        default:
                            var apiContent = await response.Content.ReadAsStringAsync();
                            finalResponseDTO = JsonConvert.DeserializeObject<ResponseDTO>(apiContent);
                            finalResponseDTO.IsSuccess = true;
                            break;
                    }
                }
                catch (Exception e)
                {
                    finalResponseDTO.ErrorMessage = new List<string>() { "Error countered:", e.Message.ToString() };
                }

                return (T)(object)finalResponseDTO;
            }
            catch (Exception e)
            {
                return default;
            }
        }


    }
}