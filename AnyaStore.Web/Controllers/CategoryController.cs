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
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [Authorize(Roles = $"{nameof(Role.Admin)}, {nameof(Role.User)}")]
        public async Task<IActionResult> CategoryIndex()
        {
            List<CategoryDTO>? categories = new();
            var claims = User.Claims.ToList();
            var response = await _categoryService.GetAllAsync<ResponseDTO>();

            if (response != null && response.IsSuccess)
            {
                categories = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(response.Result));
                return View(categories);
            }

            TempData["error"] = string.Join(", ", response?.ErrorMessage ?? new List<string>());
            return View(categories);

        }

        [Authorize(Roles = $"{nameof(Role.Admin)}")]
        public async Task<IActionResult> CategoryCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{nameof(Role.Admin)}")]
        public async Task<IActionResult> CategoryCreate(CategoryDTO category)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Category creation failed!";
                return View(category);
            }

            var response = await _categoryService.CreateAsync<ResponseDTO>(category);

            if (response?.IsSuccess == true)
            {
                TempData["success"] = "Category created successfully!";
                return RedirectToAction(nameof(CategoryIndex));
            }

            TempData["error"] = string.Join(", ", response?.ErrorMessage ?? new List<string>());
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{nameof(Role.Admin)}")]
        public async Task<IActionResult> CategoryDelete(int categoryId)
        {
            var response = await _categoryService.DeleteAsync<ResponseDTO>(categoryId);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Category deleted successfully!";
                return RedirectToAction(nameof(CategoryIndex));

            }

            TempData["error"] = string.Join(", ", response?.ErrorMessage ?? new List<string>());
            return RedirectToAction(nameof(CategoryIndex));
        }
    }
}