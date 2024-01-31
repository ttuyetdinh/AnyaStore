using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AnyaStore.Web.Ultilities.SD;

namespace AnyaStore.Web.Models.DTO
{
    public class RequestDTO
    {
        public ApiType ApiType { get; set; } = ApiType.GET;

        public string? Url { get; set; }

        public object? Data { get; set; }

        public string? AccessToken { get; set; }

        public ContentType ContentType { get; set; } = ContentType.Json;
    }
}