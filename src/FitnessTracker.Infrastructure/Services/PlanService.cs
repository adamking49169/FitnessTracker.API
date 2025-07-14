using FitnessTracker.Core.Models;
namespace FitnessTracker.Infrastructure.Services
{
    public class PlanService : IPlanService
    {
        public Task<Plan> GeneratePlanAsync(PlanRequest request)
        {
            var plan = new Plan
            {
                PlanId = Guid.NewGuid(),
                UserId = Guid.Empty,
                DailyCalorieTarget = request.Calories
            };
            return Task.FromResult(plan);
        }
    }
}
