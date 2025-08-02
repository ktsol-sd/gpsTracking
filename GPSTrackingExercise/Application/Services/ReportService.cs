using GPSTrackingExercise.Application.Interfaces;
using GPSTrackingExercise.Domain.Dtos;
using GPSTrackingExercise.Repositories.Interfaces;

namespace GPSTrackingExercise.Application.Services
{
    public class ReportService(IReportRepository _reportRepository) : IReportService
    {

        public async Task<List<ViolationsDTO>> GetViolationsAsync(int categoryId, DateTime fromTime, DateTime toTime)
        {
            return await _reportRepository.FetchViolationsAsync(categoryId, fromTime, toTime);
        }

        public async Task<List<TripDistancesDto>> GetTripDistancesAsync(int categoryId, DateTime fromTime, DateTime toTime)
        {
            return await _reportRepository.FetchTripDistancesAsync(categoryId, fromTime, toTime);
        }

        public async Task<RouteDetailsDto> GetRouteDetailsAsync(int vehicleId, DateTime fromTime, DateTime toTime)
        {
            return await _reportRepository.FetchRouteDetailsAsync(vehicleId, fromTime, toTime);
        }
    }
}
