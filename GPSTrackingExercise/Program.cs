using System;
using GPSTrackingExercise.Application.Interfaces;
using GPSTrackingExercise.Application.Services;
using GPSTrackingExercise.Infrastracture;
using GPSTrackingExercise.Repositories;
using GPSTrackingExercise.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddDbContext<GpsTrackingContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Scoped);
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<GpsSimulationService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
