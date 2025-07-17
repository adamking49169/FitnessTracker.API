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
                UserId = request.UserId,
                DailyCalorieTarget = request.Calories
            };

            _context.Plans.Add(plan);
            await _context.SaveChangesAsync();
            return plan;
        }

        public Task<Plan?> GetPlanForUserAsync(Guid userId)
        {
            return _context.Plans.FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task SavePlanAsync(Plan plan)
        {
            var exists = await _context.Plans.AnyAsync(p => p.PlanId == plan.PlanId);
            if (exists)
            {
                _context.Plans.Update(plan);
            }
            else
            {
                _context.Plans.Add(plan);
            }

            await _context.SaveChangesAsync();
        }
    }
}
