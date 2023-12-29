using Microsoft.AspNetCore.Mvc;
using OmniChat.Services;

namespace OmniChat.Controllers
{
    [Route("api/v1/data/seed")]
    public class DataSeedingController : Controller
    {
        private readonly DataSeedingService _dataSeeding;

        public DataSeedingController(DataSeedingService dataSeeding)
        {
            _dataSeeding = dataSeeding;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("seedusers")]
        public async Task<ActionResult> SeedUsersAsync()
        {
            await _dataSeeding.SeedUsersAsync();
            return Ok();
        }
    }
}