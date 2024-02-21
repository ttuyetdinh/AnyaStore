using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AnyaStore.Web.Models.DTO;
using AnyaStore.Web.Services;
using AnyaStore.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json;
using static AnyaStore.Web.Ultilities.SD;

namespace AnyaStore.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;

        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
        }

        [HttpGet]
        public IActionResult Login()
        {
            var loginDTO = new LoginRequestDTO();
            return View(loginDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDTO model)
        {
            var response = await _authService.Login<ResponseDTO>(model);

            if (response != null && response.IsSuccess)
            {
                var responseDto = JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(response.Result));

                _tokenProvider.SetToken(responseDto.Token);

                await SignInUser(responseDto);

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("CustomError", "Invalid username or password");

            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            var registerDTO = new RegistrationRequestDTO();
            var roleList = new List<SelectListItem>();

            foreach (var role in Enum.GetValues(typeof(Role)))
            {
                roleList.Add(new SelectListItem() { Text = role.ToString(), Value = role.ToString() });
            }
            ViewBag.RoleList = roleList;

            return View(registerDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationRequestDTO model)
        {

            model.Role = model.Role ?? Role.User;

            var result = await _authService.Register<ResponseDTO>(model);
            if (result != null && result.IsSuccess)
            {
                var assignRoleResult = await _authService.AssignRole<ResponseDTO>(model);
                if (assignRoleResult != null && assignRoleResult.IsSuccess)
                {
                    TempData["success"] = "Registration successfully!";
                    return RedirectToAction("login");
                }
            }

            // repopulate the role list, alternative method
            ViewBag.RoleList = Enum
                    .GetValues(typeof(Role))
                    .Cast<Role>()
                    .Select(role => new SelectListItem { Text = role.ToString(), Value = role.ToString() })
                    .ToList();

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            _tokenProvider.ClearToken();
            await SignOutUser();
            return RedirectToAction("Index", "Home");
        }

        private async Task SignInUser(LoginResponseDTO authUser)
        {
            var handler = new JsonWebTokenHandler();

            var jwt = handler.ReadJsonWebToken(authUser.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, jwt.GetClaim(JwtRegisteredClaimNames.Email).ToString()));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, jwt.GetClaim(JwtRegisteredClaimNames.Sub).ToString()));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name, jwt.GetClaim(JwtRegisteredClaimNames.Name).ToString()));

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        private async Task SignOutUser()
        {
            await HttpContext.SignOutAsync();
        }
    }
}