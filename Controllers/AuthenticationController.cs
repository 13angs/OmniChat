using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OmniChat.Configurations;
using OmniChat.Interfaces;
using OmniChat.Models;

namespace OmniChat.Controllers
{
    [Route("api/v1/auth")]
    public class AuthenticationController : Controller
    {
        private readonly IAuthService _authService;
        public AuthenticationController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<string>> Register([FromBody] RegisterRequest registerRequest)
        {
            try
            {
                AuthResponse response = await _authService.RegisterNewUserAsync(registerRequest);
                return Ok(new OkResponse<AuthResponse>
                {
                    Data = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginAsync([FromBody] LoginRequest loginRequest)
        {

            return Ok(new OkResponse<AuthResponse>
            {
                Data = await _authService.LoginAsync(loginRequest)
            });
        }
    }
}