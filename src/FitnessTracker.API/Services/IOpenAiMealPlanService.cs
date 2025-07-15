using FitnessTracker.API.Models;

namespace FitnessTracker.API.Services;

public interface IOpenAiMealPlanService
{
    Task<MealPlan> GenerateDailyMealPlanAsync(int calories, Guid userId);
}
