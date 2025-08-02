namespace GPSTrackingExercise.Domain.Models
{
    public class Violation
    {
        public string Id { get; private set; }
        public int VehicleId { get; private set; }
        public DateTime StartTime { get; private set; }
        public int DurationSeconds { get; private set; }

        private Violation() { }

        public Violation(int vehicleId, DateTime startTime, int durationSeconds)
        {
            Id = Guid.NewGuid().ToString();
            VehicleId = vehicleId;
            StartTime = startTime;
            DurationSeconds = durationSeconds;
        }
    }
}
