using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnyaStore.Web.Services.IServices;
using AnyaStore.Web.Ultilities;

namespace AnyaStore.Web.Services
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public TokenProvider(IHttpContextAccessor httpContextAccessor)
        {
            _contextAccessor = httpContextAccessor;
        }

        public void ClearToken()
        {
            _contextAccessor.HttpContext.Response.Cookies.Delete(SD.AccessToken);
        }

        public string? GetToken()
        {
            // Request.Cookie is the way togettoken in client cookie
            bool hasAccessToken = _contextAccessor.HttpContext.Request.Cookies.TryGetValue(SD.AccessToken, out string? accessToken);

            return hasAccessToken ? accessToken : null;
        }

        public void SetToken(string token)
        {
            var cookiesOptions = new CookieOptions
            {
                IsEssential = true,
                Expires = DateTime.Now.AddHours(24)
            };

            // Response.Cookie is the way to set token in client cookie
            _contextAccessor.HttpContext.Response.Cookies.Append(SD.AccessToken, token, cookiesOptions);
        }
    }
}