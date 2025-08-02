namespace GPSTrackingExercise.Application.Interfaces
{
    public interface IVehicleService
    {
        Task UploadVehiclesFromFileAsync(IFormFile file);
    }
}
