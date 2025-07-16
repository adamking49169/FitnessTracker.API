using FitnessTracker.Core.Models;
using FitnessTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Infrastructure.Services
{
    public class PlanService : IPlanService
    {
        private readonly FitnessTrackerDbContext _context;
        public PlanService(FitnessTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<Plan> GeneratePlanAsync(PlanRequest request)
        {
            var plan = new Plan
            {
                PlanId = Guid.NewGuid(),
                UserId = Guid.Empty,
                DailyCalorieTarget = request.Calories
            };
            _context.Plans.Add(plan);
            await _context.SaveChangesAsync();
            return plan;
        }

        Task<Plan> IPlanService.GeneratePlanAsync(PlanRequest request)
        {
            throw new NotImplementedException();
        }

        Task IPlanService.GetPlanForUserAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        Task IPlanService.SavePlanAsync(Plan plan)
        {
            throw new NotImplementedException();
        }
    }
}
