using JPC.Application.RainfallService;
using Microsoft.AspNetCore.Mvc;

namespace JPC.RainfallApi.Controllers
{
    // Controller
    [ApiController]
    [Route("[controller]")]
    public class RainfallController : ControllerBase
    {
        private readonly IRainfallService _rainfallService;

        public RainfallController(IRainfallService rainfallService)
        {
            _rainfallService = rainfallService;
        }

        [HttpGet("id/{stationId}/readings")]
        public async Task<IActionResult> GetRainfallReadings(string stationId, CancellationToken cancellationToken, [FromQuery] int count = 10)
        {
            var readings = await _rainfallService.GetRainfallReadingsAsync(stationId, count, cancellationToken);
            return Ok(readings);
        }
    }
}
