using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnyaStore.Web.Services.IServices
{
    public interface ITokenProvider
    {
        void SetToken(string token);
        string? GetToken();
        void ClearToken();
    }
}