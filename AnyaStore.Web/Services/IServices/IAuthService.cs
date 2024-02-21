using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnyaStore.Web.Models.DTO;

namespace AnyaStore.Web.Services.IServices
{
    public interface IAuthService
    {
        Task<T> Login<T>(LoginRequestDTO dto);
        Task<T> Register<T>(RegistrationRequestDTO dto);
        Task<T> AssignRole<T>(RegistrationRequestDTO dto);
    }
}