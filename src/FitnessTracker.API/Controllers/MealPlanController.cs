using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment _env;

        public MealPlanController(
            IOpenAiMealPlanService mealPlanService,
            IPlanService planService,
            IWebHostEnvironment env)
        {
            _mealPlanService = mealPlanService;
            _planService = planService;
            _env = env;
        }

        private bool TryGetUserId(out Guid userId)
        {
            var claim = User.FindFirst("oid");
            if (claim != null && Guid.TryParse(claim.Value, out userId))
            {
                return true;
            }
            if (_env.IsDevelopment())
            {
                userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
                return true;
            }
            userId = default;
            return false;
        }

        // from MealPlanController
        [HttpPost("generate/{calories}")]
        public async Task<ActionResult<MealPlan>> Generate(int calories)
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized();
            }
            var plan = await _mealPlanService.GenerateDailyMealPlanAsync(calories, userId);
            return Ok(plan);
        }

        // from PlanController
        [HttpGet("plan")]
        public async Task<ActionResult<Plan>> GetPlan()
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized();
            }
            var plan = await _planService.GetPlanForUserAsync(userId);
            if (plan is null) return NotFound();
            return Ok(plan);
        }

        [HttpPost("plan")]
        public async Task<IActionResult> SavePlan([FromBody] Plan plan)
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized();
            }
            plan.UserId = userId;
            await _planService.SavePlanAsync(plan);
            return NoContent();
        }
    }
}
