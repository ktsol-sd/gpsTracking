using GPSTrackingExercise.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GPSTrackingExercise.Infrastracture
{
    public class GpsTrackingContext : DbContext
    {
        public GpsTrackingContext(DbContextOptions<GpsTrackingContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<PositionEvent> PositionEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Categories");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.SpeedLimitKmh).IsRequired();
                entity.Property(e => e.ViolationDurationSec).IsRequired();
            });

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.ToTable("Vehicles");
                entity.HasKey(e => e.Id);
                entity.HasOne(v => v.Category)
                      .WithMany()
                      .HasForeignKey(v => v.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<PositionEvent>(entity =>
            {
                entity.ToTable("PositionEvents");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Timestamp).IsRequired();
                entity.Property(e => e.SpeedKmh).IsRequired();
                entity.Property(e => e.Latitude).IsRequired();
                entity.Property(e => e.Longitude).IsRequired();
            });
        }
    }
}
