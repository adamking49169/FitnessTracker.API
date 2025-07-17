using System.Net.Http.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using FitnessTracker.API.Models;
using FitnessTracker.API.Services;
using Microsoft.Azure.WebJobs;
namespace FitnessTracker.Functions;


public class ExerciseCacheLoaderFunction
{
    private readonly ICosmosExerciseService _cosmos;
    private readonly IHttpClientFactory _http;
    private readonly ILogger<ExerciseCacheLoaderFunction> _logger;

    public ExerciseCacheLoaderFunction(
        ICosmosExerciseService cosmos,
        IHttpClientFactory http,
        ILogger<ExerciseCacheLoaderFunction> logger)
    {
        _cosmos = cosmos;
        _http = http;
        _logger = logger;
    }

    [Function("ExerciseCacheLoader")]
    public async Task RunAsync([TimerTrigger("0 0 */6 * * *")] MyInfo timer)
    {
        _logger.LogInformation("Fetching exercises at: {Time}", DateTime.UtcNow);
        var client = _http.CreateClient();
        var data = await client.GetFromJsonAsync<IEnumerable<ExerciseDocument>>("https://example.com/api/exercises");
        if (data is not null)
        {
            await _cosmos.UpsertExercisesAsync(data);
            _logger.LogInformation("Upserted {Count} exercises.", data.Count());
        }
    }

    public class MyInfo { }
}
