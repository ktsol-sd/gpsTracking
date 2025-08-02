using System;
using GPSTrackingExercise.Domain.Dtos;
using GPSTrackingExercise.Infrastracture;
using GPSTrackingExercise.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GPSTrackingExercise.Repositories
{
    public class ReportRepository(GpsTrackingContext _db) : IReportRepository
    {
        public async Task<List<ViolationsDTO>> FetchViolationsAsync(int categoryId, DateTime fromDate, DateTime toDate)
        {
            var category = await _db.Categories.FindAsync(categoryId);
            if (category == null) return new List<ViolationsDTO>();

            var events = await (
                from ev in _db.PositionEvents
                join v in _db.Vehicles on ev.VehicleId equals v.Id
                where v.CategoryId == categoryId &&
                      ev.Timestamp >= fromDate &&
                      ev.Timestamp <= toDate
                orderby ev.VehicleId, ev.Timestamp
                select new
                {
                    ev.VehicleId,
                    ev.Timestamp,
                    ev.SpeedKmh
                }
            ).ToListAsync();

            var grouped = events
                .GroupBy(e => e.VehicleId)
                .Select(g =>
                {
                    int count = 0;
                    DateTime? start = null;

                    foreach (var ev in g)
                    {
                        if (ev.SpeedKmh > category.SpeedLimitKmh)
                        {
                            start ??= ev.Timestamp;
                            var duration = (ev.Timestamp - start.Value).TotalSeconds;

                            if (duration >= category.ViolationDurationSec)
                            {
                                count++;
                                start = null;
                            }
                        }
                        else
                        {
                            start = null;
                        }
                    }

                    return new ViolationsDTO
                    {
                        VehicleId = g.Key,
                        CountViolations = count
                    };
                })
                .OrderByDescending(v => v.CountViolations)
                .ToList();

            return grouped;
        }

        public async Task<List<TripDistancesDto>> FetchTripDistancesAsync(int categoryId, DateTime from, DateTime to)
        {
            var vehicleIds = await _db.Vehicles
                .Where(v => v.CategoryId == categoryId)
                .Select(v => v.Id)
                .ToListAsync();

            var allEvents = await _db.PositionEvents
                .Where(e => vehicleIds.Contains(e.VehicleId) && e.Timestamp >= from && e.Timestamp <= to)
                .OrderBy(e => e.VehicleId).ThenBy(e => e.Timestamp)
                .ToListAsync();

            var result = allEvents
                .GroupBy(e => e.VehicleId)
                .Select(g =>
                {
                    double distance = 0;
                    var list = g.ToList();
                    for (int i = 1; i < list.Count; i++)
                    {
                        distance += GeoUtils.CalculateDistanceMeters(
                            list[i - 1].Latitude, list[i - 1].Longitude,
                            list[i].Latitude, list[i].Longitude
                        );
                    }

                    return new TripDistancesDto
                    {
                        VehicleId = g.Key,
                        DistanceMeters = distance
                    };
                })
                .OrderByDescending(r => r.DistanceMeters)
                .ToList();

            return result;
        }

        public async Task<RouteDetailsDto> FetchRouteDetailsAsync(int vehicleId, DateTime from, DateTime to)
        {
            var categoryId = await _db.Vehicles
                .Where(v => v.Id == vehicleId)
                .Select(v => v.CategoryId)
                .FirstOrDefaultAsync();

            var category = await _db.Categories.FindAsync(categoryId);
            if (category == null) return null;

            var positions = await _db.PositionEvents
                .Where(e => e.VehicleId == vehicleId && e.Timestamp >= from && e.Timestamp <= to)
                .OrderBy(e => e.Timestamp)
                .ToListAsync();

            double totalDistance = 0;
            var route = new List<RouteVehicleDTO>();
            var violations = new List<ViolationInfoDto>();

            DateTime? violationStart = null;

            for (int i = 0; i < positions.Count; i++)
            {
                var ev = positions[i];
                route.Add(new RouteVehicleDTO
                {
                    Timestamp = ev.Timestamp,
                    Latitude = ev.Latitude,
                    Longitude = ev.Longitude,
                    SpeedKmh = ev.SpeedKmh
                });

                if (i > 0)
                {
                    totalDistance += GeoUtils.CalculateDistanceMeters(
                        positions[i - 1].Latitude, positions[i - 1].Longitude,
                        ev.Latitude, ev.Longitude);
                }

                if (ev.SpeedKmh > category.SpeedLimitKmh)
                {
                    violationStart ??= ev.Timestamp;
                    var duration = (ev.Timestamp - violationStart.Value).TotalSeconds;

                    if (duration >= category.ViolationDurationSec)
                    {
                        violations.Add(new ViolationInfoDto
                        {
                            StartTimestamp = violationStart.Value,
                            DurationSeconds = (int)duration
                        });
                        violationStart = null;
                    }
                }
                else
                {
                    violationStart = null;
                }
            }

            return new RouteDetailsDto
            {
                VehicleId = vehicleId,
                TripDistanceMeters = totalDistance,
                Positions = route,
                Violations = violations
            };
        }
    }
}
