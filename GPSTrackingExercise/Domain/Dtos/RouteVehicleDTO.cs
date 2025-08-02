namespace GPSTrackingExercise.Domain.Dtos
{
    public class RouteVehicleDTO
    {
        public DateTime Timestamp { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double SpeedKmh { get; set; }
    }

    public class ViolationInfoDto
    {
        public DateTime StartTimestamp { get; set; }
        public int DurationSeconds { get; set; }
    }

    public class RouteDetailsDto
    {
        public int VehicleId { get; set; }
        public double TripDistanceMeters { get; set; }
        public List<RouteVehicleDTO> Positions { get; set; }
        public List<ViolationInfoDto> Violations { get; set; }
    }
}
