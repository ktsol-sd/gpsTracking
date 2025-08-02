using System.Globalization;
using System.Text;
using GPSTrackingExercise.Application.Interfaces;
using GPSTrackingExercise.Domain.Models;
using GPSTrackingExercise.Repositories.Interfaces;

namespace GPSTrackingExercise.Application.Services
{
    public class CategoryService(IEventRepository _eventRepository) : ICategoryService
    {
        public async Task UploadCategoriesFromFileAsync(IFormFile file)
        {
            using var stream = new StreamReader(file.OpenReadStream(), Encoding.UTF8);

            var categories = new List<Category>();

            //skip header line
            var headerLine = await stream.ReadLineAsync();

            while (!stream.EndOfStream)
            {
                var line = await stream.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split('\t');
                if (parts.Length != 3) continue;

                if (!int.TryParse(parts[0], out int categoryId))
                    continue;

                if (!int.TryParse(parts[1], out int speedLimit))
                    continue;

                if (!int.TryParse(parts[2], out int speedDuration))
                    continue;

                var category = new Category(categoryId, speedLimit, speedDuration);
                categories.Add(category);
            }

            await _eventRepository.BulkInsertCategoriesAsync(categories);
        }
    }
}
