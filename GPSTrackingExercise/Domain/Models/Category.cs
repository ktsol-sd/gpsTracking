using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPSTrackingExercise.Domain.Models
{
    public class Category(int id, int speedLimitKmh, int violationDurationSec)
    {
        public int Id { get; private set; } = id;
        public int SpeedLimitKmh { get; private set; } = speedLimitKmh;
        public int ViolationDurationSec { get; private set; } = violationDurationSec;
    }
}
