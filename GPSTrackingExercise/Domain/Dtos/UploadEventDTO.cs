namespace GPSTrackingExercise.Domain.Dtos
{
    public class UploadEventDto
    {
        public Guid VehicleId { get; set; }
        public DateTime Timestamp { get; set; }
        public double SpeedKmh { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
