using OpenAI;
using FitnessTracker.API.Models;


namespace FitnessTracker.API.Services;

public class OpenAiMealPlanService : IOpenAiMealPlanService
{
    private readonly OpenAIClient _client;

    public OpenAiMealPlanService(OpenAIClient client)
    {
        _client = client;
    }

    public async Task<MealPlan> GenerateDailyMealPlanAsync(int calories, Guid userId)
    {
        var prompt = $"Generate a balanced meal plan totaling {calories} calories "
                   + "with protein, carbs, and fat breakdown for breakfast, lunch, dinner, "
                   + "and snacks.";
        var response = await _client.GetCompletionsAsync(
            deploymentOrModelName: "gpt-35-turbo",
            new CompletionsOptions { Prompt = { prompt }, MaxTokens = 512 }
        );
        var planJson = response.Value.Choices.First().Text.Trim();

        return new MealPlan
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Date = DateTime.UtcNow,
            TotalCalories = calories,
            PlanJson = planJson
        };
    }
}
