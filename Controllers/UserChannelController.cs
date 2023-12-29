using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OmniChat.Models;
using OmniChat.Services;

namespace OmniChat.Controllers
{
    [Route("api/v1/user/channel")]
    public class UserChannelController : Controller
    {
        private readonly UserChannelService _userChannelService;
        public UserChannelController(UserChannelService userChannelService)
        {
            _userChannelService = userChannelService;
        }

        [HttpPost]
        [Route("friend/add")]
        public async Task<ActionResult> AddFriend([FromBody] UserChannelRequest request)
        {
            await _userChannelService.AddFriendAsync(request);
            return StatusCode(StatusCodes.Status201Created);
        }
    }
}