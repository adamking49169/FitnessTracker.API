using FitnessTracker.Core.Models;

namespace FitnessTracker.Infrastructure.Services
{
    public interface IPlanService
    {
        Task<Plan> GeneratePlanAsync(PlanRequest request);
        Task GetPlanForUserAsync(Guid userId);
        Task SavePlanAsync(Plan plan);
    }

    public class PlanRequest
    {
        public int Calories { get; set; }
        public Guid UserId { get; set; }
    }
}
