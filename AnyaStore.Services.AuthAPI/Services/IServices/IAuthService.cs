using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnyaStore.Services.AuthAPI.Models.DTO;

namespace AnyaStore.Services.AuthAPI.Services.IServices
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationRequestDTO request);
        Task<LoginResponseDTO> Login(LoginRequestDTO request);
        Task<bool> AssignRole(string userName, string roleName);
    }
}