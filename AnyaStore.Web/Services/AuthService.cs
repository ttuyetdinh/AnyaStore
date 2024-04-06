using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnyaStore.Web.Models.DTO;
using AnyaStore.Web.Services.IServices;
using AnyaStore.Web.Ultilities;

namespace AnyaStore.Web.Services
{
    public class AuthService : IAuthService
    {
        // private readonly IHttpClientFactory _clientFactory;
        private readonly IBaseService _baseService;
        private readonly string authUrl;
        public AuthService(IHttpClientFactory clientFactory, IBaseService baseService, IConfiguration configuration)
        {
            _baseService = baseService;
            authUrl = configuration.GetValue<string>("ServiceUrls:AuthApi");
            _baseService.SetBaseUrl(authUrl);
        }


        public async Task<T> AssignRole<T>(RegistrationRequestDTO dto)
        {
            return await _baseService.SendAsync<T>(new RequestDTO()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = $"{authUrl}/api/auth/assignrole",
            });
        }

        public async Task<T> Login<T>(LoginRequestDTO dto)
        {
            return await _baseService.SendAsync<T>(new RequestDTO()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = $"{authUrl}/api/auth/login",
            }, withBearer: false);
        }

        public async Task<T> Register<T>(RegistrationRequestDTO dto)
        {
            return await _baseService.SendAsync<T>(new RequestDTO()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = $"{authUrl}/api/auth/register",
            }, withBearer: false);
        }
    }
}