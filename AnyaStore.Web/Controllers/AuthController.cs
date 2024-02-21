using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AnyaStore.Web.Models.DTO;
using AnyaStore.Web.Services;
using AnyaStore.Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using static AnyaStore.Web.Ultilities.SD;

namespace AnyaStore.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            var loginDTO = new LoginRequestDTO();
            return View(loginDTO);
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
            ViewBag.RoleList = Enum.GetValues(typeof(Role))
                                    .Cast<Role>()
                                    .Select(role => new SelectListItem { Text = role.ToString(), Value = role.ToString() })
                                    .ToList();

            return View(model);
        }

        public IActionResult Logout()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}