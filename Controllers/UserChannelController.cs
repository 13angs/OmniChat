using Microsoft.AspNetCore.Mvc;
using OmniChat.Models;
using OmniChat.Services;

namespace OmniChat.Controllers
{
    [Route("api/v1/user")]
    public class UserChannelController : Controller
    {
        private readonly UserChannelService _userChannelService;
        public UserChannelController(UserChannelService userChannelService)
        {
            _userChannelService = userChannelService;
        }

        [HttpGet]
        [Route("channels")]
        public async Task<ActionResult> GetUserChannelsAsync([FromQuery] UserChannelRequest request)
        {
            var response = await _userChannelService.GetUserChannelsAsync(request);

            return Ok(new OkResponse<UserChannelResponse>
            {
                Data = response
            });
        }

        [HttpPost]
        [Route("channel/friend/add")]
        public async Task<ActionResult> AddFriend([FromBody] UserChannelRequest request)
        {
            return StatusCode(StatusCodes.Status201Created, await _userChannelService.AddFriendAsync(request));
        }
        
        [HttpPost]
        [Route("channel/message/read")]
        public async Task<ActionResult> ReadMessageAsync([FromBody] UserChannelRequest request)
        {
            var providerId = HttpContext!.User!.FindFirst("provider_id")!.Value;
            request.ProviderId=providerId;
            return StatusCode(StatusCodes.Status200OK, await _userChannelService.ReadMessageAsync(request));
        }
    }
}