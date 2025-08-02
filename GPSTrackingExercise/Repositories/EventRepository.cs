using System;
using GPSTrackingExercise.Domain.Dtos;
using GPSTrackingExercise.Domain.Models;
using GPSTrackingExercise.Infrastracture;
using GPSTrackingExercise.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GPSTrackingExercise.Repositories
{
    public class EventRepository(GpsTrackingContext _dbContext) : IEventRepository
    {
        public async Task BulkInsertEventsAsync(List<PositionEvent> events)
        {
            await _dbContext.PositionEvents.AddRangeAsync(events);
            await _dbContext.SaveChangesAsync();
        }
        public async Task BulkInsertCategoriesAsync(List<Category> categories)
        {
            await _dbContext.Categories.AddRangeAsync(categories);
            await _dbContext.SaveChangesAsync();
        }

        public async Task BulkInsertVehiclesAsync(List<Vehicle> vehicles)
        {
            await _dbContext.Vehicles.AddRangeAsync(vehicles);
            await _dbContext.SaveChangesAsync();
        }
    }
}
