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
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [Authorize(Roles = $"{nameof(Role.Admin)}, {nameof(Role.User)}")]
        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDTO>? products = new();
            var claims = User.Claims.ToList();
            var response = await _productService.GetAllAsync<ResponseDTO>();

            if (response != null && response.IsSuccess)
            {
                products = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(response.Result));
                return View(products);
            }

            TempData["error"] = string.Join(", ", response?.ErrorMessage ?? new List<string>());
            return View(products);

        }

        [Authorize(Roles = $"{nameof(Role.Admin)}")]
        public async Task<IActionResult> ProductCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{nameof(Role.Admin)}")]
        public async Task<IActionResult> ProductCreate(ProductDTO product)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Product creation failed!";
                return View(product);
            }

            var response = await _productService.CreateAsync<ResponseDTO>(product);

            if (response?.IsSuccess == true)
            {
                TempData["success"] = "Product created successfully!";
                return RedirectToAction(nameof(ProductIndex));
            }

            TempData["error"] = string.Join(", ", response?.ErrorMessage ?? new List<string>());
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{nameof(Role.Admin)}")]
        public async Task<IActionResult> ProductDelete(int productId)
        {
            var response = await _productService.DeleteAsync<ResponseDTO>(productId);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Product deleted successfully!";
                return RedirectToAction(nameof(ProductIndex));

            }

            TempData["error"] = string.Join(", ", response?.ErrorMessage ?? new List<string>());
            return RedirectToAction(nameof(ProductIndex));
        }
    }
}