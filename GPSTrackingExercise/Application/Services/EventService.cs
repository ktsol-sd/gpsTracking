using System.Globalization;
using System.Text;
using GPSTrackingExercise.Application.Interfaces;
using GPSTrackingExercise.Domain.Models;
using GPSTrackingExercise.Repositories.Interfaces;

namespace GPSTrackingExercise.Application.Services
{
    public class EventService(IEventRepository _eventRepository) : IEventService
    {

        public async Task UploadEventsFromFileAsync(IFormFile file)
        {
            using var stream = new StreamReader(file.OpenReadStream(), Encoding.UTF8);

            var events = new List<PositionEvent>();

            bool isFirstLine = true;

            while (!stream.EndOfStream)
            {
                var line = await stream.ReadLineAsync();
                if (isFirstLine)
                {
                    isFirstLine = false;
                    continue; // skip header
                }

                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split('\t');
                if (parts.Length != 5) continue;

                try
                {
                    var vehicleId = int.Parse(parts[0]);
                    var timestamp = DateTime.Parse(parts[1], CultureInfo.InvariantCulture);
                    var speed = double.Parse(parts[2], CultureInfo.InvariantCulture);
                    var lat = double.Parse(parts[3], CultureInfo.InvariantCulture);
                    var lon = double.Parse(parts[4], CultureInfo.InvariantCulture);

                    var ev = new PositionEvent(vehicleId, timestamp, speed, lat, lon);
                    events.Add(ev);
                }
                catch (FormatException ex)
                {
                    continue;
                }
            }

            await _eventRepository.BulkInsertEventsAsync(events);
        }
    }
}
