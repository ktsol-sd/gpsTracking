using GPSTrackingExercise.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GPSTrackingExercise.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController(IReportService _reportService) : ControllerBase
    {

        [HttpGet("violations")]
        public async Task<IActionResult> GetViolations([FromQuery] int categoryId, [FromQuery] DateTime fromTime, [FromQuery] DateTime toTime)
        {
            var result = await _reportService.GetViolationsAsync(categoryId, fromTime, toTime);
            return Ok(result);
        }

        [HttpGet("trip-distances")]
        public async Task<IActionResult> GetTripDistances([FromQuery] int categoryId, [FromQuery] DateTime fromTime, [FromQuery] DateTime toTime)
        {
            var result = await _reportService.GetTripDistancesAsync(categoryId, fromTime, toTime);
            return Ok(result);
        }

        [HttpGet("route")]
        public async Task<IActionResult> GetRouteByVehicle([FromQuery] int vehicleId, [FromQuery] DateTime fromTime, [FromQuery] DateTime toTime)
        {
            var result = await _reportService.GetRouteDetailsAsync(vehicleId, fromTime, toTime);
            return Ok(result);
        }
    }
}
