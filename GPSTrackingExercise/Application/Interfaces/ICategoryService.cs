namespace GPSTrackingExercise.Application.Interfaces
{
    public interface ICategoryService
    {
        Task UploadCategoriesFromFileAsync(IFormFile file);
    }
}
