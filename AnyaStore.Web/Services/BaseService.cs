using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
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
        private readonly ITokenProvider _tokenProvider;

        public BaseService(IHttpClientFactory httpClientFactory, IApiMessageRequestBuilder apiMessageRequestBuilder, IConfiguration configuration, ILogger<BaseService> logger, ITokenProvider tokenProvider)
        {
            _httpClientFactory = httpClientFactory;
            _apiMessageRequestBuilder = apiMessageRequestBuilder;
            _logger = logger;
            _tokenProvider = tokenProvider;
        }
        public void SetBaseUrl(string baseUrl)
        {
            BaseUrl = baseUrl ?? "";
        }
        public async Task<T> SendAsync<T>(RequestDTO requestDTO, bool withBearer = true)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("AnyaStoreAPI");

                var message = _apiMessageRequestBuilder.Build(requestDTO);
                // add bearer token
                if (withBearer)
                {
                    var token = _tokenProvider.GetToken();
                    if (!string.IsNullOrEmpty(token))
                    {
                        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }
                }

                var response = await client.SendAsync(message);
                var apiContent = await response.Content.ReadAsStringAsync();

                ResponseDTO finalResponseDTO = new()
                {
                    IsSuccess = false
                };

                try
                {
                    // todo: refactor http response cases
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.NotFound:
                            finalResponseDTO.StatusCode = HttpStatusCode.NotFound;
                            finalResponseDTO.ErrorMessage = new List<string>() { "Not found" };
                            _logger.LogError($"{requestDTO.ApiType}, {requestDTO.Url}: {apiContent}");
                            break;

                        case HttpStatusCode.Forbidden:
                            finalResponseDTO.StatusCode = HttpStatusCode.Forbidden;
                            finalResponseDTO.ErrorMessage = new List<string>() { "Access denied" };
                            _logger.LogError($"{requestDTO.ApiType}, {requestDTO.Url}: {apiContent}");
                            break;

                        case HttpStatusCode.Unauthorized:
                            finalResponseDTO.StatusCode = HttpStatusCode.Unauthorized;
                            finalResponseDTO.ErrorMessage = new List<string>() { "Unauthorized", };
                            _logger.LogError($"{requestDTO.ApiType}, {requestDTO.Url}: {apiContent}");
                            break;

                        case HttpStatusCode.InternalServerError:
                            finalResponseDTO.StatusCode = HttpStatusCode.InternalServerError;
                            finalResponseDTO.ErrorMessage = new List<string>() { "Internal server error" };
                            _logger.LogError($"{requestDTO.ApiType}, {requestDTO.Url}: {apiContent}");
                            break;

                        case HttpStatusCode.BadRequest:
                            var tempDTO = JsonConvert.DeserializeObject<ResponseDTO>(apiContent);
                            finalResponseDTO.StatusCode = HttpStatusCode.BadRequest;

                            if (tempDTO == null || tempDTO.StatusCode == 0)
                            {
                                finalResponseDTO.ErrorMessage = new List<string> { "Bad Request" };
                                _logger.LogError($"{requestDTO.ApiType}, {requestDTO.Url}: {apiContent}");
                            }
                            else
                            {
                                finalResponseDTO = tempDTO;
                                _logger.LogError($"{requestDTO.ApiType}, {requestDTO.Url}: {finalResponseDTO.ErrorMessage.FirstOrDefault()}");
                            }
                            break;

                        default:
                            finalResponseDTO = JsonConvert.DeserializeObject<ResponseDTO>(apiContent);
                            finalResponseDTO.IsSuccess = true;
                            _logger.LogInformation($"{requestDTO.ApiType}, {requestDTO.Url}: Response received successfully.");
                            break;
                    }

                }
                catch (Exception e)
                {
                    finalResponseDTO.ErrorMessage = new List<string>() { "Error countered:", e.Message.ToString() };
                    _logger.LogError($"{requestDTO.ApiType}, {requestDTO.Url}: {finalResponseDTO.ErrorMessage.FirstOrDefault()}");
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