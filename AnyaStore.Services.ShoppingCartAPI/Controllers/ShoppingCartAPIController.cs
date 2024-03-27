using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AnyaStore.Services.ShoppingCartAPI.Migrations;
using AnyaStore.Services.ShoppingCartAPI.Models;
using AnyaStore.Services.ShoppingCartAPI.Models.DTO;
using AnyaStore.Services.ShoppingCartAPI.Repository.IRepository;
using AnyaStore.Services.ShoppingCartAPI.Services.IServices;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static AnyaStore.Services.ShoppingCartAPI.Ultilities.SD;

namespace AnyaStore.Services.ShoppingCartAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartAPIController : ControllerBase
    {
        private readonly ICartHeaderRepository _cartHeaderRepository;
        private readonly ICartDetailRepository _cartDetailRepository;
        private readonly IProductService _productService;
        private readonly ICouponService _couponService;
        private ResponseDTO _responseDTO;
        private readonly IMapper _mapper;

        public ShoppingCartAPIController(ICartHeaderRepository cartHeaderRepository, IMapper mapper, ICartDetailRepository cartDetailRepository, IProductService productService, ICouponService couponService)
        {
            _cartHeaderRepository = cartHeaderRepository;
            _responseDTO = new ResponseDTO();
            _mapper = mapper;
            _cartDetailRepository = cartDetailRepository;
            _productService = productService;
            _couponService = couponService;
        }

        [HttpGet("GetCart/{userId}")]
        // [Authorize(Roles = $"{nameof(Role.Admin)},{nameof(Role.User)}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ResponseDTO>> GetCart(string userId)
        {
            try
            {
                var cartHeader = await _cartHeaderRepository.GetAsync(u => u.UserId == userId, includeProperties: "CartDetails");
                var cart = new CartDTO()
                {
                    CartHeader = _mapper.Map<CartHeaderDTO>(cartHeader),
                    CartDetails = _mapper.Map<List<CartDetailsDTO>>(cartHeader.CartDetails)
                };

                var productList = await _productService.GetProducts();

                foreach (var item in cart.CartDetails)
                {
                    item.CartHeader = null;
                    item.Product = productList.FirstOrDefault(u => u.ProductId == item.ProductId);
                    cart.CartHeader.CartTotal += item.Count * (item?.Product?.Price ?? 0);

                }

                if (!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
                {
                    var coupon = await _couponService.GetCoupon(cart.CartHeader.CouponCode);
                    if (coupon != null && cart.CartHeader.CartTotal >= coupon.MinAmount)
                    {
                        cart.CartHeader.Discount = cart.CartHeader.CartTotal * (coupon.DiscountAmount ?? 0) / 100;
                        cart.CartHeader.CartTotal -= cart.CartHeader.Discount;
                    }
                }

                _responseDTO.Result = cart;
                _responseDTO.StatusCode = HttpStatusCode.OK;

                return Ok(_responseDTO);
            }
            catch (Exception ex)
            {
                _responseDTO = ErrorResponse(ex);
                return BadRequest(_responseDTO);
            }
        }

        [HttpPost("ApplyCoupon")]
        public async Task<ActionResult<ResponseDTO>> ApplyCoupon(CartHeaderDTO entity)
        {
            try
            {
                var cartHeader = await _cartHeaderRepository.GetAsync(u => u.CartHeaderId == entity.CartHeaderId);
                if (cartHeader == null)
                {
                    _responseDTO.ErrorMessage = new List<string> { "CartHeader not found." };
                    _responseDTO.IsSuccess = false;
                    _responseDTO.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responseDTO);
                }

                cartHeader.CouponCode = entity.CouponCode;
                await _cartHeaderRepository.UpdateAsync(cartHeader);

                _responseDTO.Result = entity;
                _responseDTO.StatusCode = HttpStatusCode.OK;
                _responseDTO.IsSuccess = true;
                return Ok(_responseDTO);
            }
            catch (Exception ex)
            {
                _responseDTO = ErrorResponse(ex);
                return BadRequest(_responseDTO);
            }
        }

        [HttpPost("{cartHeaderId:int}/RemoveCoupon")]
        public async Task<ActionResult<ResponseDTO>> RemoveCoupon(int cartHeaderId)
        {
            try
            {
                var cartHeader = await _cartHeaderRepository.GetAsync(u => u.CartHeaderId == cartHeaderId);
                if (cartHeader == null)
                {
                    _responseDTO.ErrorMessage = new List<string> { "CartHeader not found." };
                    _responseDTO.IsSuccess = false;
                    _responseDTO.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responseDTO);
                }

                cartHeader.CouponCode = "";
                await _cartHeaderRepository.UpdateAsync(cartHeader);

                _responseDTO.Result = cartHeader;
                _responseDTO.StatusCode = HttpStatusCode.OK;
                _responseDTO.IsSuccess = true;
                return Ok(_responseDTO);
            }
            catch (Exception ex)
            {
                _responseDTO = ErrorResponse(ex);
                return BadRequest(_responseDTO);
            }
        }

        [HttpPost("CartUpsert")]
        public async Task<ActionResult<ResponseDTO>> CartUpsert(CartUpsertDTO entity)
        {
            try
            {
                var cartDetailsDTO = entity.CartDetails.First();

                var cartHeader = await _cartHeaderRepository.GetAsync(u => u.UserId == entity.CartHeader.UserId, tracked: false, includeProperties: "CartDetails");
                if (cartHeader == null)
                {
                    //  create a new cart header
                    CartHeader newCartheader = _mapper.Map<CartHeader>(entity.CartHeader);
                    await _cartHeaderRepository.CreateAsync(newCartheader);

                    cartDetailsDTO.CartHeaderId = newCartheader.CartHeaderId;
                    cartDetailsDTO.Order = 1;
                    await _cartDetailRepository.CreateAsync(_mapper.Map<CartDetails>(cartDetailsDTO));
                }
                else
                {
                    // if cart header is not null, check if cart detail already have the product
                    var cartDetails = await _cartDetailRepository.GetAsync(u => u.ProductId == cartDetailsDTO.ProductId
                                                                                && u.CartHeaderId == cartHeader.CartHeaderId, tracked: false);
                    if (cartDetails == null)
                    {
                        // add a new product to cart detail
                        cartDetailsDTO.CartHeaderId = cartHeader.CartHeaderId;
                        cartDetailsDTO.Order = cartHeader.CartDetails.Count + 1;
                        await _cartDetailRepository.CreateAsync(_mapper.Map<CartDetails>(cartDetailsDTO));
                    }
                    else
                    {
                        // update the cart detail
                        cartDetailsDTO.Count += cartDetails.Count;
                        cartDetailsDTO.Order = cartDetails.Order;
                        cartDetailsDTO.CartDetailsId = cartDetails.CartDetailsId;
                        cartDetailsDTO.CartHeaderId = cartDetails.CartHeaderId;
                        await _cartDetailRepository.UpdateAsync(_mapper.Map<CartDetails>(cartDetailsDTO));
                    }
                }
                entity.CartDetails = new List<CartDetailsUpsertDTO>() { cartDetailsDTO };
                _responseDTO.Result = entity;
                _responseDTO.StatusCode = HttpStatusCode.OK;

                return Ok(_responseDTO);
            }
            catch (Exception ex)
            {
                _responseDTO = ErrorResponse(ex);
                return BadRequest(_responseDTO);
            }
        }

        [HttpPost("RemoveFromCart")]
        public async Task<ActionResult<ResponseDTO>> RemoveFromCart(CartDetailsRemoveDTO entity)
        {
            try
            {
                var cartDetails = await _cartDetailRepository.GetAsync(u => u.CartDetailsId == entity.CartDetailsId);
                if (cartDetails == null)
                {
                    return NotFound(new ResponseDTO
                    {
                        ErrorMessage = new List<string> { "An error occurred while retrieving the cartDetails.", "CartDetails not found." },
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound
                    });
                }

                var cartHeader = await _cartHeaderRepository.GetAsync(u => u.CartHeaderId == cartDetails.CartHeaderId, includeProperties: "CartDetails");
                int numberItems = cartHeader.CartDetails.Count;


                if (numberItems == 1)
                {
                    // cardHeader has delete on cascade
                    await _cartHeaderRepository.RemoveAsync(cartHeader);
                }
                else
                {
                    await _cartDetailRepository.RemoveAsync(cartDetails);
                }

                _responseDTO.Result = entity;
                _responseDTO.StatusCode = HttpStatusCode.OK;
                _responseDTO.IsSuccess = true;
                return Ok(_responseDTO);
            }
            catch (Exception ex)
            {
                _responseDTO = ErrorResponse(ex);
                return BadRequest(_responseDTO);
            }
        }

        // ultilities methos
        private ResponseDTO ErrorResponse(Exception ex)
        {
            return new ResponseDTO
            {
                ErrorMessage = new List<string>() {
                    "An error occurred while processing your request.",
                    ex.InnerException != null ? ex.InnerException.Message : ex.Message
                },
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest
            };
        }
    }
}