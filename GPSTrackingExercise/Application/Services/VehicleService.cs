using System.Text;
using GPSTrackingExercise.Application.Interfaces;
using GPSTrackingExercise.Domain.Models;
using GPSTrackingExercise.Repositories.Interfaces;

namespace GPSTrackingExercise.Application.Services
{
    public class VehicleService(IEventRepository _eventRepository) : IVehicleService
    {
        public async Task UploadVehiclesFromFileAsync(IFormFile file)
        {
            using var stream = new StreamReader(file.OpenReadStream(), Encoding.UTF8);

            var vehicles = new List<Vehicle>();

            // Skip header line
            var headerLine = await stream.ReadLineAsync();

            while (!stream.EndOfStream)
            {
                var line = await stream.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split('\t');
                if (parts.Length != 2) continue;

                if (!int.TryParse(parts[0], out int vehicleId)) continue;
                if (!int.TryParse(parts[1], out int categoryId)) continue;

                var vehicle = new Vehicle(vehicleId, categoryId);
                vehicles.Add(vehicle);
            }

            await _eventRepository.BulkInsertVehiclesAsync(vehicles);
        }
    }
}
