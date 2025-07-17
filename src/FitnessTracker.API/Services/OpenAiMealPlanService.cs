using OpenAI;
using OpenAI.Chat;
using FitnessTracker.Core.Models;


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
        var chatClient = _client.GetChatClient("gpt-3.5-turbo");
        ChatCompletionOptions options = new() { MaxOutputTokenCount = 512 };

        ChatCompletion response = await chatClient.CompleteChatAsync(
            new[] { new UserChatMessage(prompt) },
            options);
        var planJson = response.Content[0].Text.Trim();
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
