using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AnyaStore.Web.Models.DTO;
using AnyaStore.Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static AnyaStore.Web.Ultilities.SD;

namespace AnyaStore.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;

        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        [Authorize(Roles = $"{nameof(Role.Admin)}, {nameof(Role.User)}")]
        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDTO>? coupons = new();
            var claims = User.Claims.ToList();
            var response = await _couponService.GetAllAsync<ResponseDTO>();

            if (response != null && response.IsSuccess)
            {
                coupons = JsonConvert.DeserializeObject<List<CouponDTO>>(Convert.ToString(response.Result));
                return View(coupons);
            }

            TempData["error"] = string.Join(", ", response?.ErrorMessage ?? new List<string>());
            return View(coupons);

        }

        [Authorize(Roles = $"{nameof(Role.Admin)}")]
        public async Task<IActionResult> CouponCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{nameof(Role.Admin)}")]
        public async Task<IActionResult> CouponCreate(CouponDTO coupon)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Coupon creation failed!";
                return View(coupon);
            }

            var response = await _couponService.CreateAsync<ResponseDTO>(coupon);

            if (response?.IsSuccess == true)
            {
                TempData["success"] = "Coupon created successfully!";
                return RedirectToAction(nameof(CouponIndex));
            }

            TempData["error"] = string.Join(", ", response?.ErrorMessage ?? new List<string>());
            return View(coupon);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{nameof(Role.Admin)}")]
        public async Task<IActionResult> CouponDelete(int couponId)
        {
            var response = await _couponService.DeleteAsync<ResponseDTO>(couponId);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Coupon deleted successfully!";
                return RedirectToAction(nameof(CouponIndex));

            }

            TempData["error"] = string.Join(", ", response?.ErrorMessage ?? new List<string>());
            return RedirectToAction(nameof(CouponIndex));
        }
    }
}