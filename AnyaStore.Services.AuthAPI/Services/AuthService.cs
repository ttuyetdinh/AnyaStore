using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnyaStore.Services.AuthAPI.Data;
using AnyaStore.Services.AuthAPI.Models;
using AnyaStore.Services.AuthAPI.Models.DTO;
using AnyaStore.Services.AuthAPI.Services.IServices;
using Microsoft.AspNetCore.Identity;

namespace AnyaStore.Services.AuthAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AuthService(AppDbContext appDbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = appDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<LoginResponseDTO> Login(LoginRequestDTO request)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == request.UserName);
            // the CheckPasswordAsync return false if user is null
            bool isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

            if (user == null || !isPasswordValid)
            {
                return new LoginResponseDTO
                {
                    Token = null,
                    User = null
                };
            }

            // if user valid, generate JWT token
            var userDTO = new UserDTO
            {
                ID = user.Id,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            return new LoginResponseDTO
            {
                User = userDTO,
                Token = ""
            };
        }

        public async Task<string> Register(RegistrationRequestDTO request)
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                NormalizedEmail = request.Email.ToUpper(),
                Name = request.Name,
                PhoneNumber = request.PhoneNumber
            };

            try
            {
                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    return "";
                }
                else
                {
                    return result.Errors.Select(x => x.Description).FirstOrDefault();
                }
            }
            catch (System.Exception e)
            {
                return $"Registration Failed, {e}";
            }
        }
    }
}