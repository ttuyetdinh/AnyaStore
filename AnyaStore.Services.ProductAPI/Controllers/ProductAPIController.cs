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
    public class ProductAPIController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private ResponseDTO _responseDTO;
        private readonly IMapper _mapper;

        public ProductAPIController(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _responseDTO = new ResponseDTO();
            _mapper = mapper;

        }

        [HttpGet]
        // [Authorize(Roles = $"{nameof(Role.Admin)},{nameof(Role.User)}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ResponseDTO>> GetProducts()
        {
            try
            {
                var includeProperties = "Category";
                var products = await _productRepository.GetAllAsync(includeProperties: includeProperties);
                var productsDTO = _mapper.Map<List<ProductDTO>>(products);

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

        [HttpGet("{id:int}")]
        // [Authorize(Roles = $"{nameof(Role.Admin)},{nameof(Role.User)}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetProduct(int id)
        {
            try
            {
                var includeProperties = "Category";
                var product = await _productRepository.GetAsync(i => i.ProductId == id, includeProperties: includeProperties);
                var productDTO = _mapper.Map<ProductDTO>(product);

                // check if product exists
                if (productDTO == null)
                {
                    _responseDTO.ErrorMessage = new List<string>() { "Product not found." };
                    _responseDTO.IsSuccess = false;
                    _responseDTO.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responseDTO);
                }

                _responseDTO.Result = productDTO;
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


        // create a http post to insert new product
        [HttpPost]
        [Authorize(Roles = $"{nameof(Role.Admin)}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDTO productDTO)
        {
            try
            {
                var product = _mapper.Map<Product>(productDTO);
                await _productRepository.CreateAsync(product);

                _responseDTO.Result = _mapper.Map<ProductDTO>(product);
                _responseDTO.StatusCode = HttpStatusCode.OK;

                return Ok(_responseDTO);
            }
            catch (Exception ex)
            {
                _responseDTO.ErrorMessage = new List<string>() {
                    "An error occurred while creating the product.",
                    ex.InnerException != null ? ex.InnerException.Message : ex.Message
                };
                _responseDTO.IsSuccess = false;
                _responseDTO.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_responseDTO);
            }
        }

        // create a http put to update product
        [HttpPut]
        [Authorize(Roles = $"{nameof(Role.Admin)}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductDTO productDTO)
        {
            try
            {
                var product = await _productRepository.GetAsync(i => i.ProductId == productDTO.ProductId);
                if (product == null)
                {
                    _responseDTO.ErrorMessage = new List<string>() { "Product not found." };
                    _responseDTO.IsSuccess = false;
                    _responseDTO.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responseDTO);
                }
                // this won' work becasue create a new instance of product which is not tracked by entity framework
                // product = _mapper.Map<Product>(productDTO);
                // use this instead
                _mapper.Map(productDTO, product);
                await _productRepository.UpdateAsync(product);

                _responseDTO.Result = productDTO;
                _responseDTO.StatusCode = HttpStatusCode.OK;

                return Ok(_responseDTO);
            }
            catch (Exception ex)
            {
                _responseDTO.ErrorMessage = new List<string>() {
                    "An error occurred while updating the product.",
                    ex.InnerException != null ? ex.InnerException.Message : ex.Message
                };
                _responseDTO.IsSuccess = false;
                _responseDTO.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_responseDTO);
            }
        }

        // create a http delete to delete product
        [HttpDelete("{id:int}")]
        [Authorize(Roles = $"{nameof(Role.Admin)}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _productRepository.GetAsync(i => i.ProductId == id);
                if (product == null)
                {
                    _responseDTO.ErrorMessage = new List<string>() { "Product not found." };
                    _responseDTO.IsSuccess = false;
                    _responseDTO.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responseDTO);
                }
                await _productRepository.RemoveAsync(product);

                _responseDTO.Result = "Delete successful.";
                _responseDTO.StatusCode = HttpStatusCode.OK;

                return Ok(_responseDTO);
            }
            catch (Exception ex)
            {
                _responseDTO.ErrorMessage = new List<string>() {
                    "An error occurred while deleting the product.",
                    ex.InnerException != null ? ex.InnerException.Message : ex.Message
                };
                _responseDTO.IsSuccess = false;
                _responseDTO.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_responseDTO);
            }
        }
    }
}