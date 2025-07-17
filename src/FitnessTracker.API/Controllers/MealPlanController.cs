using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FitnessTracker.Core.Models;
using FitnessTracker.API.Services;
using FitnessTracker.Infrastructure.Services; // for IPlanService

namespace FitnessTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MealPlanController : ControllerBase
    {
        private readonly IOpenAiMealPlanService _mealPlanService;
        private readonly IPlanService _planService;

        public MealPlanController(
            IOpenAiMealPlanService mealPlanService,
            IPlanService planService)
        {
            _mealPlanService = mealPlanService;
            _planService = planService;
        }

        // from MealPlanController
        [HttpPost("generate/{calories}")]
        public async Task<ActionResult<MealPlan>> Generate(int calories)
        {
            var userId = Guid.Parse(User.FindFirst("oid")!.Value);
            var plan = await _mealPlanService.GenerateDailyMealPlanAsync(calories, userId);
            return Ok(plan);
        }

        // from PlanController
        [HttpGet("plan")]
        public async Task<ActionResult<Plan>> GetPlan()
        {
            var userId = Guid.Parse(User.FindFirst("oid")!.Value);
            var plan = await _planService.GetPlanForUserAsync(userId);
            if (plan is null) return NotFound();
            return Ok(plan);
        }

        [HttpPost("plan")]
        public async Task<IActionResult> SavePlan([FromBody] Plan plan)
        {
            plan.UserId = Guid.Parse(User.FindFirst("oid")!.Value);
            await _planService.SavePlanAsync(plan);
            return NoContent();
        }
    }
}
