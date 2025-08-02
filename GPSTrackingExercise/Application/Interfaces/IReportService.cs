using GPSTrackingExercise.Domain.Dtos;

namespace GPSTrackingExercise.Application.Interfaces
{
    public interface IReportService
    {
        Task<List<ViolationsDTO>> GetViolationsAsync(int categoryId, DateTime fromTime, DateTime toTime);
        Task<List<TripDistancesDto>> GetTripDistancesAsync(int categoryId, DateTime fromTime, DateTime toTime);
        Task<RouteDetailsDto> GetRouteDetailsAsync(int vehicleId, DateTime fromTime, DateTime toTime);
    }
}
