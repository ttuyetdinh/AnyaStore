using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AnyaStore.Services.CouponAPI.Models;
using AnyaStore.Services.CouponAPI.Models.DTO;
using AnyaStore.Services.CouponAPI.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnyaStore.Services.CouponAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CouponAPIController : ControllerBase
    {
        private readonly ICouponRepository _couponRepository;
        private ResponseDTO _responseDTO;
        private readonly IMapper _mapper;

        public CouponAPIController(ICouponRepository couponRepository, IMapper mapper)
        {
            _couponRepository = couponRepository;
            _responseDTO = new ResponseDTO();
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<ActionResult<ResponseDTO>> GetCoupons()
        {
            try
            {
                var coupons = await _couponRepository.GetAllAsync();
                var couponsDTO = _mapper.Map<List<CouponDTO>>(coupons);

                _responseDTO.Result = couponsDTO;
                _responseDTO.StatusCode = HttpStatusCode.OK;

                return Ok(_responseDTO);
            }
            catch (Exception ex)
            {
                _responseDTO.ErrorMessage = new List<string>() {
                    "An error occurred while retrieving the coupons.",
                    ex.Message
                };
                _responseDTO.IsSuccess = false;
                _responseDTO.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_responseDTO);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCoupon(int id)
        {
            try
            {
                var coupon = await _couponRepository.GetAsync(i => i.CouponId == id);
                var couponDTO = _mapper.Map<CouponDTO>(coupon);

                // check if coupon exists
                if (couponDTO == null)
                {
                    _responseDTO.ErrorMessage = new List<string>() { "Coupon not found." };
                    _responseDTO.IsSuccess = false;
                    _responseDTO.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responseDTO);
                }

                _responseDTO.Result = couponDTO;
                _responseDTO.StatusCode = HttpStatusCode.OK;

                return Ok(_responseDTO);
            }
            catch (Exception ex)
            {
                _responseDTO.ErrorMessage = new List<string>() {
                    "An error occurred while retrieving the coupons.",
                    ex.Message
                };
                _responseDTO.IsSuccess = false;
                _responseDTO.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_responseDTO);
            }
        }

        [HttpGet("GetByCode/{code}")]
        public async Task<IActionResult> GetCouponByCode(string code)
        {
            try
            {
                var coupon = await _couponRepository.GetAsync(i => i.CouponCode.ToLower().Contains(code.ToLower()));
                var couponDTO = _mapper.Map<CouponDTO>(coupon);

                // check if coupon exists
                if (couponDTO == null)
                {
                    _responseDTO.ErrorMessage = new List<string>() { "Coupon not found." };
                    _responseDTO.IsSuccess = false;
                    _responseDTO.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responseDTO);
                }

                _responseDTO.Result = couponDTO;
                _responseDTO.StatusCode = HttpStatusCode.OK;

                return Ok(_responseDTO);
            }
            catch (Exception ex)
            {
                _responseDTO.ErrorMessage = new List<string>() {
                    "An error occurred while retrieving the coupons.",
                    ex.Message
                };
                _responseDTO.IsSuccess = false;
                _responseDTO.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_responseDTO);
            }
        }


        // create a http post to insert new coupon
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateCoupon([FromBody] CouponDTO couponDTO)
        {
            try
            {
                var coupon = _mapper.Map<Coupon>(couponDTO);
                await _couponRepository.CreateAsync(coupon);

                _responseDTO.Result = couponDTO;
                _responseDTO.StatusCode = HttpStatusCode.OK;

                return Ok(_responseDTO);
            }
            catch (Exception ex)
            {
                _responseDTO.ErrorMessage = new List<string>() {
                    "An error occurred while creating the coupon.",
                    ex.Message
                };
                _responseDTO.IsSuccess = false;
                _responseDTO.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_responseDTO);
            }
        }

        // create a http put to update coupon
        [HttpPut]
        public async Task<IActionResult> UpdateCoupon([FromBody] CouponDTO couponDTO)
        {
            try
            {
                var coupon = await _couponRepository.GetAsync(i => i.CouponId == couponDTO.CouponId);
                if (coupon == null)
                {
                    _responseDTO.ErrorMessage = new List<string>() { "Coupon not found." };
                    _responseDTO.IsSuccess = false;
                    _responseDTO.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responseDTO);
                }
                // this won' work becasue create a new instance of coupon which is not tracked by entity framework
                // coupon = _mapper.Map<Coupon>(couponDTO);
                // use this instead
                _mapper.Map(couponDTO, coupon);
                await _couponRepository.UpdateAsync(coupon);

                _responseDTO.Result = couponDTO;
                _responseDTO.StatusCode = HttpStatusCode.OK;

                return Ok(_responseDTO);
            }
            catch (Exception ex)
            {
                _responseDTO.ErrorMessage = new List<string>() {
                    "An error occurred while updating the coupon.",
                    ex.Message
                };
                _responseDTO.IsSuccess = false;
                _responseDTO.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_responseDTO);
            }
        }
        // create a http delete to delete coupon
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCoupon(int id)
        {
            try
            {
                var coupon = await _couponRepository.GetAsync(i => i.CouponId == id);
                if (coupon == null)
                {
                    _responseDTO.ErrorMessage = new List<string>() { "Coupon not found." };
                    _responseDTO.IsSuccess = false;
                    _responseDTO.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responseDTO);
                }
                await _couponRepository.RemoveAsync(coupon);

                _responseDTO.Result = true;
                _responseDTO.StatusCode = HttpStatusCode.OK;

                return Ok(_responseDTO);
            }
            catch (Exception ex)
            {
                _responseDTO.ErrorMessage = new List<string>() {
                    "An error occurred while deleting the coupon.",
                    ex.Message
                };
                _responseDTO.IsSuccess = false;
                _responseDTO.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_responseDTO);
            }
        }
    }
}