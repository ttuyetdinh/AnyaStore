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
using Newtonsoft.Json;
using static AnyaStore.Web.Ultilities.SD;

namespace AnyaStore.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
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
            var categories = await _categoryService.GetAllAsync<ResponseDTO>();
            var productCreateVM = new ProductCreateVM();
            productCreateVM = await PopulateCategories(productCreateVM);
            return View(productCreateVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{nameof(Role.Admin)}")]
        public async Task<IActionResult> ProductCreate(ProductCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Product creation failed!";
                model = await PopulateCategories(model);
                return View(model);
            }

            var response = await _productService.CreateAsync<ResponseDTO>(model.Product);

            if (response?.IsSuccess == true)
            {
                TempData["success"] = "Product created successfully!";
                return RedirectToAction(nameof(ProductIndex));
            }

            TempData["error"] = string.Join(", ", response?.ErrorMessage ?? new List<string>());

            // repopulate the categories
            model = await PopulateCategories(model);
            return View(model);
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


        // helper methods
        private async Task<ProductCreateVM> PopulateCategories(ProductCreateVM model)
        {
            try
            {
                var categories = await _categoryService.GetAllAsync<ResponseDTO>();
                if (categories != null && categories.IsSuccess)
                {
                    model.Categories = JsonConvert
                        .DeserializeObject<List<CategoryDTO>>(Convert.ToString(categories.Result))
                        .Select(i => new SelectListItem
                        {
                            Text = i.Name,
                            Value = i.CategoryId.ToString()
                        });
                }
            }
            catch (Exception e)
            {

                model.Categories = new List<SelectListItem>();
            }
            return model;
        }
    }
}