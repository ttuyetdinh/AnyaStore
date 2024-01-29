using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnyaStore.Services.CouponAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace AnyaStore.Services.CouponAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CouponAPIController : ControllerBase
    {
        private readonly ICouponRepository _couponRepository;

        public CouponAPIController(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCoupons()
        {
            try
            {
                var coupons = await _couponRepository.GetAllAsync();
                return Ok(coupons);
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred while retrieving the coupons.");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCoupon(int id)
        {
            try
            {
                var coupons = await _couponRepository.GetAsync(i => i.CouponId == id);
                return Ok(coupons);
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred while retrieving the coupon.");
            }
        }
    }
}