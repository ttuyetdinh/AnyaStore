using AnyaStore.Web.Models;
using AnyaStore.Web.Models.DTO;
using AnyaStore.Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace AnyaStore.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public HomeController(ILogger<HomeController> logger, ICategoryService categoryService, IProductService productService)
        {
            _logger = logger;
            _categoryService = categoryService;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            List<ProductDTO>? products = new();
            var response = await _productService.GetAllAsync<ResponseDTO>();

            if (response != null && response.IsSuccess)
            {
                products = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(response.Result));
                return View(products);
            }

            TempData["error"] = string.Join(", ", response?.ErrorMessage ?? new List<string>());
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
