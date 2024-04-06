using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AnyaStore.Web.Models.DTO;
using AnyaStore.Web.Models.ViewModel;
using AnyaStore.Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json;
using static AnyaStore.Web.Ultilities.SD;

namespace AnyaStore.Web.Controllers
{
    public class CartController : Controller
    {

        private readonly IShoppingCartService _cartService;
        public CartController(IShoppingCartService cartService)
        {
            _cartService = cartService;
        }

        [Authorize]
        public async Task<IActionResult> CartIndex()
        {
            var cart = new CartDTO();
            var minAmount = 0;
            var discount = 0.0;

            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value;
            var response = await _cartService.GetCartByUserAsync<ResponseDTO>(userId);

            if (response != null && response.IsSuccess)
            {
                cart = JsonConvert.DeserializeObject<CartDTO>(Convert.ToString(response.Result));
                if (cart?.CartHeader?.Coupon != null)
                {
                    minAmount = cart.CartHeader.Coupon?.MinAmount ?? 0;
                    discount = cart.CartHeader.Coupon?.DiscountAmount ?? 0;
                }
                cart.CartHeader.CartTotal = cart.CartDetails.Sum(x => x.Product.Price * x.Count);
                cart.CartHeader.Discount = minAmount < cart.CartHeader.CartTotal ? cart.CartHeader.CartTotal * discount / 100 : 0;
                cart.CartHeader.FinalTotal = cart.CartHeader.CartTotal - cart.CartHeader.Discount;

                return View(cart);
            }

            TempData["error"] = string.Join(", ", response?.ErrorMessage ?? new List<string>());
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDTO cartDTO)
        {
            var UserId = cartDTO.CartHeader.UserId;
            var cartId = cartDTO.CartHeader.CartHeaderId ?? 0;

            var response = await _cartService.ApplyCouponAsync<ResponseDTO>(cartId, cartDTO.CartHeader);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Coupon applied successfully!";
            }
            else
            {
                TempData["error"] = string.Join(", ", response?.ErrorMessage ?? new List<string>());
            }
            return RedirectToAction(nameof(CartIndex));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDTO cartDTO)
        {
            string cartHeaderId = cartDTO.CartHeader?.CartHeaderId?.ToString() ?? string.Empty;

            var response = await _cartService.RemoveCouponAsync<ResponseDTO>(cartHeaderId);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Coupon remove successfully!";
            }
            else
            {
                TempData["error"] = string.Join(", ", response?.ErrorMessage ?? new List<string>());
            }
            return RedirectToAction(nameof(CartIndex));
        }
    }
}