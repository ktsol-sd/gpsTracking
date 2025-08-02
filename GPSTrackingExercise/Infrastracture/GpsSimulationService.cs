using System;
using GPSTrackingExercise.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GPSTrackingExercise.Infrastracture
{
    public class GpsSimulationService(IServiceScopeFactory _scopeFactory, ILogger<GpsSimulationService> _logger) : BackgroundService
    {
        private readonly Random _random = new();
        private const int DelaySeconds = 10;

        private const double MinLatitude = 37.831167;
        private const double MaxLatitude = 38.205395;
        private const double MinLongitude = 23.407745;
        private const double MaxLongitude = 24.130096;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("GPS Simulation Service started");
            await Task.Delay(1000, stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                var startTime = DateTime.UtcNow;
                try
                {
                    await SimulatePositions(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in GPS simulation");
                }

                var elapsed = DateTime.UtcNow - startTime;
                var delay = TimeSpan.FromSeconds(DelaySeconds) - elapsed;
                if (delay > TimeSpan.Zero)
                    await Task.Delay(delay, stoppingToken);
            }
        }

        private async Task SimulatePositions(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<GpsTrackingContext>();

            var vehicles = await db.Vehicles.ToListAsync(stoppingToken);
            if (!vehicles.Any())
            {
                return;
            }

            var lastPositions = await db.PositionEvents
                .Where(e => vehicles.Select(v => v.Id).Contains(e.VehicleId))
                .GroupBy(e => e.VehicleId)
                .Select(g => g.OrderByDescending(e => e.Timestamp).First())
                .ToListAsync(stoppingToken);

            var lastDict = lastPositions.ToDictionary(p => p.VehicleId, p => p);

            var now = DateTime.UtcNow;
            var newEvents = new List<PositionEvent>();

            foreach (var vehicle in vehicles)
            {
                lastDict.TryGetValue(vehicle.Id, out var last);
                var newPos = GenerateNearbyPosition(last, vehicle.Id, now);
                if (newPos != null)
                    newEvents.Add(newPos);
            }

            await db.PositionEvents.AddRangeAsync(newEvents, stoppingToken);
            await db.SaveChangesAsync(stoppingToken);
            _logger.LogInformation("Simulated positions for {Count} vehicles", newEvents.Count);
        }

        private PositionEvent? GenerateNearbyPosition(PositionEvent? last, int vehicleId, DateTime now)
        {
            try
            {
                // Generate random distance [0,50) meters
                double distanceMeters = _random.NextDouble() * 50;
                double bearing = _random.NextDouble() * 2 * Math.PI;
                const double metersToDegrees = 1.0 / 111320.0;

                double prevLat = last?.Latitude ?? RandomLat();
                double prevLng = last?.Longitude ?? RandomLng();

                double deltaLat = Math.Cos(bearing) * distanceMeters * metersToDegrees;
                double deltaLng = Math.Sin(bearing) * distanceMeters * metersToDegrees / Math.Cos(prevLat * Math.PI / 180);

                double newLat = Math.Clamp(prevLat + deltaLat, MinLatitude, MaxLatitude);
                double newLng = Math.Clamp(prevLng + deltaLng, MinLongitude, MaxLongitude);

                // Compute speed in km/h
                double speed = (distanceMeters / DelaySeconds) * 3.6;

                return new PositionEvent(vehicleId, now, newLat, newLng, speed);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to generate position for vehicle {VehicleId}", vehicleId);
                return null;
            }
        }

        private double RandomLat() => MinLatitude + _random.NextDouble() * (MaxLatitude - MinLatitude);
        private double RandomLng() => MinLongitude + _random.NextDouble() * (MaxLongitude - MinLongitude);
    }
}