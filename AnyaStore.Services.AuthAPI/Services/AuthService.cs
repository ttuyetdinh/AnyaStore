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
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(AppDbContext appDbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _db = appDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<bool> AssignRole(string userName, string roleName)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == userName);
            if (user == null) return false;

            // create role if not exist
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }

            // assign role to user
            await _userManager.AddToRoleAsync(user, roleName);
            return true;
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
                    User = null,
                    Token = null
                };
            }

            // if user valid, generate JWT token
            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenGenerator.GenerateToken(user, roles);

            return new LoginResponseDTO
            {
                User = new UserDTO
                {
                    ID = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                },
                Token = token
            };
        }

        public async Task<string> Register(RegistrationRequestDTO request)
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                NormalizedEmail = request.Email.ToUpper(),
                Name = request.Name ?? request.Email.Split('@').FirstOrDefault(),
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