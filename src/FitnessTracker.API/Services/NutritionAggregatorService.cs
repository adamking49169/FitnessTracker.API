using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using System.Net.Http.Json;
using FitnessTracker.Core.Models;
using FitnessTracker.Infrastructure.Services;

namespace FitnessTracker.API.Services;

public class NutritionAggregatorService : INutritionAggregatorService
{
    private readonly IHttpClientFactory _http;
    private readonly IConfiguration _cfg;
    private readonly AsyncRetryPolicy _retryPolicy;
    private readonly AsyncCircuitBreakerPolicy _cbPolicy;

    public NutritionAggregatorService(IHttpClientFactory http, IConfiguration cfg)
    {
        _http = http;
        _cfg = cfg;
        _retryPolicy = Policy.Handle<Exception>()
            .WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(2));
        _cbPolicy = Policy.Handle<Exception>()
            .CircuitBreakerAsync(2, TimeSpan.FromMinutes(1));
    }

    public async Task<FoodEntry> GetMacrosForBarcodeAsync(string barcode, Guid userId)
    {
        // example: call Nutritionix first, fallback to Edamam
        var client1 = _http.CreateClient();
        FoodEntry? entry = null;

        try
        {
            await _cbPolicy.ExecuteAsync(() =>
                _retryPolicy.ExecuteAsync(async () =>
                {
                    // Nutritionix call
                    var url = $"https://api.nutritionix.com/v1_1/item?upc={barcode}"
                              + $"&appId={_cfg["NutritionApis:NutritionixAppId"]}"
                              + $"&appKey={_cfg["NutritionApis:NutritionixAppKey"]}";
                    var resp = await client1.GetAsync(url);
                    resp.EnsureSuccessStatusCode();
                    var apiEntry = await resp.Content.ReadFromJsonAsync<FoodEntry>();
                    if (apiEntry is not null) entry = apiEntry;
                }));
        }
        catch
        {
            var client2 = _http.CreateClient();
            var url = $"https://api.edamam.com/api/food-database/v2/parser?upc={barcode}"
                      + $"&app_id={_cfg["NutritionApis:EdamamAppId"]}"
                      + $"&app_key={_cfg["NutritionApis:EdamamAppKey"]}";
            var resp2 = await client2.GetAsync(url);
            if (resp2.IsSuccessStatusCode)
            {
                entry = await resp2.Content.ReadFromJsonAsync<FoodEntry>();
            }
        }

        if (entry is null)
            throw new Exception("Failed to retrieve nutrition info from both services");

        entry.Id = Guid.NewGuid();
        entry.UserId = userId;
        entry.Date = DateTime.UtcNow;
        entry.Calories = entry.Protein * 4 + entry.Carbs * 4 + entry.Fat * 9;
        return entry;
    }

    public Task<FoodEntry> CreateManualEntryAsync(double protein, double carbs, double fat, Guid userId)
    {
        var entry = new FoodEntry
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Date = DateTime.UtcNow,
            Protein = protein,
            Carbs = carbs,
            Fat = fat,
            Calories = protein * 4 + carbs * 4 + fat * 9
        };
        return Task.FromResult(entry);
    }
}
