using System.ComponentModel.DataAnnotations;

namespace GPSTrackingExercise.Domain.Models
{
    public class PositionEvent
    {
        public Guid Id { get; private set; }
        public int VehicleId { get; private set; }
        public DateTime Timestamp { get; private set; }
        public double SpeedKmh { get; private set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        private PositionEvent() { }

        public PositionEvent(int vehicleId, DateTime timestamp, double speedKmh, double latitude, double longitude)
        {
            Id = Guid.NewGuid();
            VehicleId = vehicleId;
            Timestamp = timestamp;
            SpeedKmh = speedKmh;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
