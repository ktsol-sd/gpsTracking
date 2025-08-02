using GPSTrackingExercise.Domain.Dtos;

namespace GPSTrackingExercise.Repositories.Interfaces
{
    public interface IReportRepository
    {
        Task<List<ViolationsDTO>> FetchViolationsAsync(int categoryId, DateTime from, DateTime to);
        Task<List<TripDistancesDto>> FetchTripDistancesAsync(int categoryId, DateTime from, DateTime to);
        Task<RouteDetailsDto> FetchRouteDetailsAsync(int vehicleId, DateTime from, DateTime to);
    }
}
