using FitnessTracker.Infrastructure.Services;
using FitnessTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using FitnessTracker.Infrastructure.Services;
using FitnessTracker.Infrastructure;      // or the correct namespace for INutritionService


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<FitnessTrackerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IPlanService, PlanService>();
// Infrastructure services
builder.Services.AddScoped<INutritionService, NutritionService>();
builder.Services.AddScoped<IPlanService, PlanService>();

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
