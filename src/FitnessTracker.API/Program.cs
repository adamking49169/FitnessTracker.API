// Program.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.Azure.Cosmos;
using OpenAI;
//using FitnessTracker.API.Data;
using FitnessTracker.API.Services;
using FitnessTracker.Infrastructure.Data;
using FitnessTracker.Infrastructure.Services;
using FitnessTracker.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
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
var sqlConn = Environment.GetEnvironmentVariable("SQLSERVER_CONNECTION")
              ?? builder.Configuration.GetConnectionString("SqlServer");
builder.Services.AddDbContext<FitnessTrackerDbContext>(opt =>
    opt.UseSqlServer(sqlConn));

// 3) HTTP client
builder.Services.AddHttpClient();

// 4) Cosmos DB (optional)
var cosmosConn = Environment.GetEnvironmentVariable("COSMOS_CONNECTION")
                 ?? builder.Configuration.GetConnectionString("CosmosDb");
if (!string.IsNullOrWhiteSpace(cosmosConn) && !cosmosConn.Contains("COSMOS_CONNECTION_STRING"))
{
    builder.Services.AddSingleton(_ => new CosmosClient(cosmosConn));
    builder.Services.AddScoped<ICosmosExerciseService, CosmosExerciseService>();
}

// 5) OpenAI
var openAiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")
                ?? builder.Configuration["OpenAI:ApiKey"]!;
builder.Services.AddSingleton(_ =>
    new OpenAIClient(openAiKey));

// 6) Your application services
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
        var allowAll = new AuthorizationPolicyBuilder()
            .RequireAssertion(_ => true)
            .Build();
        options.DefaultPolicy = allowAll;
        options.FallbackPolicy = allowAll;
    });
}
else
{
    // Prod (or Staging): real JWT
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            var cfg = builder.Configuration;
            var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? cfg["Jwt:Key"]!;
            var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? cfg["Jwt:Issuer"];
            var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? cfg["Jwt:Audience"];
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                                              Encoding.UTF8.GetBytes(jwtKey)),
                ValidateIssuer = true,
                ValidIssuer = jwtIssuer,
                ValidateAudience = true,
                ValidAudience = jwtAudience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });

    builder.Services.AddAuthorization();
}

var app = builder.Build();

// Apply EF Core migrations automatically in development
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<FitnessTrackerDbContext>();
    //db.Database.Migrate();
    await DbSeeder.SeedAsync(db);
}

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
