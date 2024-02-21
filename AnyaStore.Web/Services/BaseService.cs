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

        private readonly ILogger<BaseService> _logger;

        public BaseService(IHttpClientFactory httpClientFactory, IApiMessageRequestBuilder apiMessageRequestBuilder, IConfiguration configuration, ILogger<BaseService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _apiMessageRequestBuilder = apiMessageRequestBuilder;
            _logger = logger;
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
                            _logger.LogError($"{requestDTO.ApiType} {requestDTO.Url}", finalResponseDTO.ErrorMessage);
                            break;
                        case HttpStatusCode.Forbidden:
                            finalResponseDTO.ErrorMessage = new List<string>() { "Access denied" };
                            _logger.LogError($"{requestDTO.ApiType}, {requestDTO.Url}: {finalResponseDTO.ErrorMessage}");
                            break;
                        case HttpStatusCode.Unauthorized:
                            finalResponseDTO.ErrorMessage = new List<string>() { "Unauthorized" };
                            _logger.LogError($"{requestDTO.ApiType}, {requestDTO.Url}: {finalResponseDTO.ErrorMessage}");
                            break;
                        case HttpStatusCode.InternalServerError:
                            finalResponseDTO.ErrorMessage = new List<string>() { "Internal server error" };
                            _logger.LogError($"{requestDTO.ApiType}, {requestDTO.Url}: {finalResponseDTO.ErrorMessage}");
                            break;
                        case HttpStatusCode.BadRequest:
                            var error = await response.Content.ReadAsStringAsync();
                            finalResponseDTO.ErrorMessage = new List<string> { JsonConvert.DeserializeObject<ResponseDTO>(error).ToString() };
                            _logger.LogError($"{requestDTO.ApiType}, {requestDTO.Url}: {finalResponseDTO.ErrorMessage}");
                            break;
                        default:
                            var apiContent = await response.Content.ReadAsStringAsync();
                            finalResponseDTO = JsonConvert.DeserializeObject<ResponseDTO>(apiContent);
                            finalResponseDTO.IsSuccess = true;
                            _logger.LogInformation($"{requestDTO.ApiType}, {requestDTO.Url}: Response received successfully.");
                            break;
                    }

                }
                catch (Exception e)
                {
                    finalResponseDTO.ErrorMessage = new List<string>() { "Error countered:", e.Message.ToString() };
                    _logger.LogError($"{requestDTO.ApiType}, {requestDTO.Url}: {finalResponseDTO.ErrorMessage}");
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