using Microsoft.AspNetCore.Mvc;
using OmniChat.Models;
using OmniChat.Services;

namespace OmniChat.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class ChannelController : ControllerBase
    {
        private readonly ChannelService _channelService;
        public ChannelController(ChannelService channelService)
        {
            _channelService = channelService;
        }

        [HttpPost]
        [Route("channel/create")]
        public async Task<ActionResult> CreateChannelAsync([FromBody] ChannelRequest request)
        {
            // Get the current HttpContext
            var httpContext = HttpContext;

            // Get the base URL without the path and query string
            var baseUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}";

            return StatusCode(StatusCodes.Status201Created, await _channelService.CreateChannelAsync(request, baseUrl));
        }
    }
}