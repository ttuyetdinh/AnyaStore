using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyaStore.Web.Models.DTO;
using AnyaStore.Web.Services.IServices;
using Newtonsoft.Json;
using static AnyaStore.Web.Ultilities.SD;

namespace AnyaStore.Web.Services
{
    public class ApiMessageRequestBuilder : IApiMessageRequestBuilder
    {
        public HttpRequestMessage Build(RequestDTO requestDTO)
        {
            HttpRequestMessage message = new HttpRequestMessage
            {
                RequestUri = new Uri(requestDTO.Url)
            };

            if (requestDTO.ContentType == ContentType.MultipartFormData)
            {
                message.Headers.Add("Accept", "*/*");
                var content = new MultipartFormDataContent();
                foreach (var prop in requestDTO.Data.GetType().GetProperties())
                {
                    var value = prop.GetValue(requestDTO.Data);
                    if (value is FormFile)
                    {
                        var file = (FormFile)value;
                        if (file != null) content.Add(new StreamContent(file.OpenReadStream()), prop.Name, file.FileName);
                    }
                    else
                    {
                        content.Add(new StringContent(value == null ? "" : value.ToString()), prop.Name);
                    }
                }
                message.Content = content;
            }
            else
            {
                message.Headers.Add("Accept", "application/json");

                if (requestDTO.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(requestDTO.Data), Encoding.UTF8, "application/json");
                }
            }
            message.Method = GetHttpMethod(requestDTO.ApiType);

            return message;
        }
        // ultilities function
        private HttpMethod GetHttpMethod(ApiType apiType)
        {
            return apiType switch
            {
                ApiType.GET => HttpMethod.Get,
                ApiType.POST => HttpMethod.Post,
                ApiType.PUT => HttpMethod.Put,
                ApiType.DELETE => HttpMethod.Delete,
                _ => HttpMethod.Get,
            };
        }
    }

}