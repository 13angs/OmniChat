using Microsoft.AspNetCore.Mvc;
using OmniChat.Interfaces;
using OmniChat.Models;

namespace OmniChat.Controllers
{
    [Route("api/v1/users")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
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
    }
}