// Program.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.Azure.Cosmos;
using OpenAI;
//using FitnessTracker.API.Data;
using FitnessTracker.API.Services;
using FitnessTracker.Infrastructure.Data;
using FitnessTracker.Infrastructure.Services;
using FitnessTracker.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1) Core services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2) EF Core (SQL Server)
builder.Services.AddDbContext<FitnessTrackerDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

// 3) HTTP client
builder.Services.AddHttpClient();

// 4) Cosmos DB (single connection-string)
var cosmosConn = builder.Configuration.GetConnectionString("CosmosDb");
builder.Services.AddSingleton(_ => new CosmosClient(cosmosConn));

// 5) OpenAI
builder.Services.AddSingleton(_ =>
    new OpenAIClient(builder.Configuration["OpenAI:ApiKey"]!));

// 6) Your application services
builder.Services.AddScoped<ICosmosExerciseService, CosmosExerciseService>();
builder.Services.AddScoped<INutritionAggregatorService, NutritionAggregatorService>();
builder.Services.AddScoped<IOpenAiMealPlanService, OpenAiMealPlanService>();
builder.Services.AddScoped<INutritionService, NutritionService>();
builder.Services.AddScoped<IPlanService, PlanService>();

// 7) AuthN/Z
if (builder.Environment.IsDevelopment())
{
    // Dev: allow EVERYTHING, no JWT key needed
    builder.Services.AddAuthorization(options =>
    {
        options.FallbackPolicy = new AuthorizationPolicyBuilder()
            .RequireAssertion(_ => true)
            .Build();
    });
}
else
{
    // Prod (or Staging): real JWT
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            var cfg = builder.Configuration;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                                              Encoding.UTF8.GetBytes(cfg["Jwt:Key"]!)),
                ValidateIssuer = true,
                ValidIssuer = cfg["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = cfg["Jwt:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });

    builder.Services.AddAuthorization();
}

var app = builder.Build();

// 8) Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
    // **NO** UseAuthentication/UseAuthorization here
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
}

app.MapControllers();

app.Run();
