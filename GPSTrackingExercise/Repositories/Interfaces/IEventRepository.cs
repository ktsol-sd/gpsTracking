using GPSTrackingExercise.Domain.Models;

namespace GPSTrackingExercise.Repositories.Interfaces
{
    public interface IEventRepository
    {
        Task BulkInsertEventsAsync(List<PositionEvent> events);
        Task BulkInsertCategoriesAsync(List<Category> categories);
        Task BulkInsertVehiclesAsync(List<Vehicle> vehicles);
    }
}
