using Microsoft.EntityFrameworkCore;
using Microsoft.Azure.Cosmos;
using OpenAI;
//using FitnessTracker.API.Data;
using FitnessTracker.API.Services;
using FitnessTracker.Infrastructure.Data;
using FitnessTracker.Infrastructure.Services;
using FitnessTracker.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<FitnessTrackerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient();
builder.Services.AddSingleton(new CosmosClient(builder.Configuration["Cosmos:ConnectionString"]));
builder.Services.AddSingleton(new OpenAIClient(builder.Configuration["OpenAI:Key"]));

builder.Services.AddScoped<ICosmosExerciseService, CosmosExerciseService>();
builder.Services.AddScoped<INutritionAggregatorService, NutritionAggregatorService>();
builder.Services.AddScoped<IOpenAiMealPlanService, OpenAiMealPlanService>();
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
