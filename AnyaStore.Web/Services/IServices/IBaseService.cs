using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnyaStore.Web.Models.DTO;

namespace AnyaStore.Web.Services.IServices
{
    public interface IBaseService
    {
        string BaseUrl { get; }
        Task<T> SendAsync<T>(RequestDTO requestDTO, bool withBearer = true);
        void SetBaseUrl(string baseUrl);
    }
}