using Microsoft.AspNetCore.Mvc;
using OmniChat.Interfaces;
using OmniChat.Models;

namespace OmniChat.Controllers
{
    [Route("api/v1/provider")]
    public class ProviderController : Controller
    {
        private readonly IProviderService _providerService;
        public ProviderController(IProviderService providerService)
        {
            _providerService = providerService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult> CreateProvider([FromBody] CreateProviderRequest request)
        {
            try
            {
                await _providerService.CreateProviderAsync(request);
                return StatusCode(StatusCodes.Status201Created);
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