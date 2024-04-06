using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AnyaStore.Services.ProductAPI.Migrations;
using AnyaStore.Services.ProductAPI.Models;
using AnyaStore.Services.ProductAPI.Models.DTO;
using AnyaStore.Services.ProductAPI.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static AnyaStore.Services.ProductAPI.Ultilities.SD;

namespace AnyaStore.Services.ProductAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryAPIController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private ResponseDTO _responseDTO;
        private readonly IMapper _mapper;

        public CategoryAPIController(ICategoryRepository categoryRepository, IMapper mapper, IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _responseDTO = new ResponseDTO();
            _mapper = mapper;
            _productRepository = productRepository;
        }

        [HttpGet]
        // [Authorize(Roles = $"{nameof(Role.Admin)},{nameof(Role.User)}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ResponseDTO>> GetCategories()
        {
            try
            {
                var categories = await _categoryRepository.GetAllAsync();
                var categoriesDTO = _mapper.Map<List<CategoryDTO>>(categories);

                _responseDTO.Result = categoriesDTO;
                _responseDTO.StatusCode = HttpStatusCode.OK;

                return Ok(_responseDTO);
            }
            catch (Exception ex)
            {
                _responseDTO.ErrorMessage = new List<string>() {
                    "An error occurred while retrieving the categories.",
                    ex.InnerException != null ? ex.InnerException.Message : ex.Message
                };
                _responseDTO.IsSuccess = false;
                _responseDTO.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_responseDTO);
            }
        }

        [HttpGet("{id:int}")]
        // [Authorize(Roles = $"{nameof(Role.Admin)},{nameof(Role.User)}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCategory(int id)
        {
            try
            {
                var category = await _categoryRepository.GetAsync(i => i.CategoryId == id);
                var categoryDTO = _mapper.Map<CategoryDTO>(category);

                // check if category exists
                if (categoryDTO == null)
                {
                    _responseDTO.ErrorMessage = new List<string>() { "Category not found." };
                    _responseDTO.IsSuccess = false;
                    _responseDTO.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responseDTO);
                }

                _responseDTO.Result = categoryDTO;
                _responseDTO.StatusCode = HttpStatusCode.OK;

                return Ok(_responseDTO);
            }
            catch (Exception ex)
            {
                _responseDTO.ErrorMessage = new List<string>() {
                    "An error occurred while retrieving the categories.",
                    ex.InnerException != null ? ex.InnerException.Message : ex.Message
                };
                _responseDTO.IsSuccess = false;
                _responseDTO.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_responseDTO);
            }
        }

        [HttpGet("{id:int}/products")]
        // [Authorize(Roles = $"{nameof(Role.Admin)},{nameof(Role.User)}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetProductByCategory(int id)
        {
            try
            {
                var includeProperties = "Category";
                var products = await _productRepository.GetAllAsync(i => i.CategoryId == id, includeProperties: includeProperties);

                var productsDTO = _mapper.Map<List<ProductDTO>>(products);

                // check if product exists
                if (productsDTO.Count == 0)
                {
                    _responseDTO.ErrorMessage = new List<string>() { "Product not found." };
                    _responseDTO.IsSuccess = false;
                    _responseDTO.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responseDTO);
                }

                _responseDTO.Result = productsDTO;
                _responseDTO.StatusCode = HttpStatusCode.OK;

                return Ok(_responseDTO);
            }
            catch (Exception ex)
            {
                _responseDTO.ErrorMessage = new List<string>() {
                    "An error occurred while retrieving the products.",
                    ex.InnerException != null ? ex.InnerException.Message : ex.Message
                };
                _responseDTO.IsSuccess = false;
                _responseDTO.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_responseDTO);
            }
        }

        // create a http post to insert new category
        [HttpPost]
        [Authorize(Roles = $"{nameof(Role.Admin)}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDTO categoryDTO)
        {
            try
            {
                var category = _mapper.Map<Category>(categoryDTO);
                await _categoryRepository.CreateAsync(category);

                _responseDTO.Result = _mapper.Map<CategoryDTO>(category);
                _responseDTO.StatusCode = HttpStatusCode.OK;

                return Ok(_responseDTO);
            }
            catch (Exception ex)
            {
                _responseDTO.ErrorMessage = new List<string>() {
                    "An error occurred while creating the category.",
                    ex.InnerException != null ? ex.InnerException.Message : ex.Message
                };
                _responseDTO.IsSuccess = false;
                _responseDTO.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_responseDTO);
            }
        }

        // create a http put to update category
        [HttpPut]
        [Authorize(Roles = $"{nameof(Role.Admin)}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryDTO categoryDTO)
        {
            try
            {
                var category = await _categoryRepository.GetAsync(i => i.CategoryId == categoryDTO.CategoryId);
                if (category == null)
                {
                    _responseDTO.ErrorMessage = new List<string>() { "Category not found." };
                    _responseDTO.IsSuccess = false;
                    _responseDTO.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responseDTO);
                }
                // this won' work becasue create a new instance of category which is not tracked by entity framework
                // category = _mapper.Map<Category>(categoryDTO);
                // use this instead
                _mapper.Map(categoryDTO, category);
                await _categoryRepository.UpdateAsync(category);

                _responseDTO.Result = categoryDTO;
                _responseDTO.StatusCode = HttpStatusCode.OK;

                return Ok(_responseDTO);
            }
            catch (Exception ex)
            {
                _responseDTO.ErrorMessage = new List<string>() {
                    "An error occurred while updating the category.",
                    ex.InnerException != null ? ex.InnerException.Message : ex.Message
                };
                _responseDTO.IsSuccess = false;
                _responseDTO.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_responseDTO);
            }
        }

        // create a http delete to delete category
        [HttpDelete("{id:int}")]
        [Authorize(Roles = $"{nameof(Role.Admin)}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var category = await _categoryRepository.GetAsync(i => i.CategoryId == id);
                if (category == null)
                {
                    _responseDTO.ErrorMessage = new List<string>() { "Category not found." };
                    _responseDTO.IsSuccess = false;
                    _responseDTO.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responseDTO);
                }
                await _categoryRepository.RemoveAsync(category);

                _responseDTO.Result = "Delete successful.";
                _responseDTO.StatusCode = HttpStatusCode.OK;

                return Ok(_responseDTO);
            }
            catch (Exception ex)
            {
                _responseDTO.ErrorMessage = new List<string>() {
                    "An error occurred while deleting the category.",
                    ex.InnerException != null ? ex.InnerException.Message : ex.Message
                };
                _responseDTO.IsSuccess = false;
                _responseDTO.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_responseDTO);
            }
        }
    }
}