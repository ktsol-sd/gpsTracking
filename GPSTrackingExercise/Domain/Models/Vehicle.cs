using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPSTrackingExercise.Domain.Models
{
    public class Vehicle
    {
        public int Id { get; private set; }
        public int CategoryId { get; private set; }
        public Category Category { get; private set; }
        public Vehicle(int id, int categoryId)
        {
            Id = id;
            CategoryId = categoryId;
        }
    }
}
