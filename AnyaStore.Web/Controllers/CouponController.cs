using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AnyaStore.Web.Models.DTO;
using AnyaStore.Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AnyaStore.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;

        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDTO>? coupons = new();

            var response = await _couponService.GetAllAsync<ResponseDTO>();

            if (response != null && response.IsSuccess)
            {
                coupons = JsonConvert.DeserializeObject<List<CouponDTO>>(Convert.ToString(response.Result));
            }

            return View(coupons);
        }

        public async Task<IActionResult> CouponCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CouponCreate(CouponDTO coupon)
        {
            if (ModelState.IsValid)
            {
                var response = await _couponService.CreateAsync<ResponseDTO>(coupon);

                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(CouponIndex));
                }
            }


            return View(coupon);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CouponDelete(int couponId)
        {
            var response = await _couponService.DeleteAsync<ResponseDTO>(couponId);

            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(CouponIndex));

            }

            return RedirectToAction(nameof(CouponIndex));
        }

        // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        // public IActionResult Error()
        // {
        //     return View("Error!");
        // }
    }
}