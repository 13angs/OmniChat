using Microsoft.AspNetCore.Mvc;
using OmniChat.Interfaces;
using OmniChat.Models;

namespace OmniChat.Controllers
{
    [Route("api/v1")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("users")]
        public async Task<ActionResult> GetUsers([FromQuery] UserRequest request)
        {
            try
            {
                return Ok(new OkResponse<UserResponse>
                {
                    Data = await _userService.GetUsersAsync(request)
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

        [HttpPost]
        [Route("user/me")]
        public async Task<ActionResult> GetMyProfileAsync([FromBody] UserRequest request)
        {
            return Ok(new OkResponse<UserResponse>
            {
                Data = await _userService.GetMyProfileAsync(request)
            });
        }
    }
}