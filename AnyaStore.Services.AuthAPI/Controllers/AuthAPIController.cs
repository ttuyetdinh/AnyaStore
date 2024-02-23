using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AnyaStore.Services.AuthAPI.Models.DTO;
using AnyaStore.Services.AuthAPI.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace AnyaStore.Services.AuthAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        protected ResponseDTO _response;

        public AuthAPIController(IAuthService authService)
        {
            _authService = authService;
            this._response = new ResponseDTO();
        }


        [HttpGet("test")]
        public IActionResult Test()
        {
            _response.Result = "Auth API is working";
            return Ok(_response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO request)
        {
            var result = await _authService.Register(request);
            if (!string.IsNullOrEmpty(result))
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string> { result };
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = "User created successfully!";
            return Ok(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            var result = await _authService.Login(request);
            if (result.Token == null)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string> { "Invalid login attempt" };
                _response.StatusCode = HttpStatusCode.Unauthorized;
                return Unauthorized(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = result;
            return Ok(_response);
        }

        [HttpPost("assignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDTO request)
        {
            // Email is used as UserName
            var result = await _authService.AssignRole(request.Email, request.Role.ToString());
            if (!result)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string> { "Failed to assign role" };
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = "Role assigned successfully!";
            return Ok(_response);
        }
    }
}