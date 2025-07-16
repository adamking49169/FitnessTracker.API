using FitnessTracker.API.Models;

namespace FitnessTracker.API.Services;

public interface ICosmosExerciseService
{
    Task UpsertExercisesAsync(IEnumerable<ExerciseDocument> exercises);
    Task<IEnumerable<ExerciseDocument>> GetAllAsync();
}
