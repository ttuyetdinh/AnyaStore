using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnyaStore.Services.ShoppingCartAPI.Ultilities
{
    public static class SD
    {
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        };
        public enum ContentType
        {
            Json,
            MultipartFormData
        };

        public enum Role
        {
            Admin,
            User,
            CustomRole
        };
        public static string CouponAPIBase { get; set; }
        public static string AuthAPIBase { get; set; }
        public static string AccessToken = "JWTToken";
        public static string RefreshToken = "RefreshToken";
    }
}