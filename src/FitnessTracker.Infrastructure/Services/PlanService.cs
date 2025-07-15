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
    }
}
