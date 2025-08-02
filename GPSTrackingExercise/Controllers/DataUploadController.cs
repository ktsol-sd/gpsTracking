using GPSTrackingExercise.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GPSTrackingExercise.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataUploadController(IEventService _eventService, ICategoryService _categoryService, IVehicleService _vehicleService) : ControllerBase
    {

        [HttpPost("categories")]
        public async Task<IActionResult> UploadCategories([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Category file is required.");

            await _categoryService.UploadCategoriesFromFileAsync(file);
            return Ok("Categories uploaded successfully.");
        }

        [HttpPost("vehicles")]
        public async Task<IActionResult> UploadVehicles([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Vehicle file is required.");

            await _vehicleService.UploadVehiclesFromFileAsync(file);
            return Ok("Vehicles uploaded successfully.");
        }

        [HttpPost("events")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadEvents([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Event file is required.");

            await _eventService.UploadEventsFromFileAsync(file);
            return Ok("Events uploaded successfully.");
        }
    }
}
