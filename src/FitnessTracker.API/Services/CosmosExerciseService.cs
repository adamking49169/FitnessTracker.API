using Microsoft.Azure.Cosmos;
using FitnessTracker.API.Models;

namespace FitnessTracker.API.Services;

public class CosmosExerciseService : ICosmosExerciseService
{
    private readonly Container _container;

    public CosmosExerciseService(CosmosClient client, IConfiguration cfg)
    {
        _container = client
            .GetDatabase(cfg["Cosmos:Database"])
            .GetContainer(cfg["Cosmos:Container"]);
    }

    public async Task UpsertExercisesAsync(IEnumerable<ExerciseDocument> exercises)
    {
        foreach (var ex in exercises)
        {
            await _container.UpsertItemAsync(ex, new PartitionKey(ex.Type));
        }
    }

    public async Task<IEnumerable<ExerciseDocument>> GetAllAsync()
    {
        var query = _container.GetItemQueryIterator<ExerciseDocument>("SELECT * FROM c");
        var list = new List<ExerciseDocument>();
        while (query.HasMoreResults)
            list.AddRange(await query.ReadNextAsync());
        return list;
    }
}
