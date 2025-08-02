namespace GPSTrackingExercise.Application.Interfaces
{
    public interface IEventService
    {
        Task UploadEventsFromFileAsync(IFormFile file);
    }
}
