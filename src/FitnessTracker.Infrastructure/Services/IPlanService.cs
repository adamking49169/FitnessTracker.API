using FitnessTracker.Core.Models;
namespace FitnessTracker.Infrastructure.Services
{
    public interface IPlanService
    {
        Task<Plan> GeneratePlanAsync(PlanRequest request);
    }

    public class PlanRequest
    {
        public int Calories { get; set; }
    }
}
