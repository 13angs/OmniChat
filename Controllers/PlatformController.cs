using Microsoft.AspNetCore.Mvc;
using OmniChat.Models;
using OmniChat.Services;

namespace OmniChat.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class PlatformController : ControllerBase
    {
        private readonly PlatformService _platformService;
        public PlatformController(PlatformService platformService)
        {
            _platformService = platformService;
        }
        [HttpGet]
        [Route("platforms")]
        public ActionResult GetPlatforms()
        {

            return Ok(new OkResponse<PlatformResponse>
            {
                Data = _platformService.GetPlatforms()
            });
        }
    }
}