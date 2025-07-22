using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using FitnessTracker.Core.Models;
using FitnessTracker.API.Extensions;
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


        // from MealPlanController
        [HttpPost("generate/{calories}")]
        public async Task<ActionResult<MealPlan>> Generate(int calories)
        {
            var userId = User.GetUserId(_env.IsDevelopment());
            if (userId is null)
            {
                return Unauthorized();
            }
            var plan = await _mealPlanService.GenerateDailyMealPlanAsync(calories, userId.Value);
            return Ok(plan);
        }

        // from PlanController
        [HttpGet("plan")]
        public async Task<ActionResult<Plan>> GetPlan()
        {
            var userId = User.GetUserId(_env.IsDevelopment());
            if (userId is null)
            {
                return Unauthorized();
            }
            var plan = await _planService.GetPlanForUserAsync(userId.Value);
            if (plan is null) return NotFound();
            return Ok(plan);
        }

        [HttpPost("plan")]
        public async Task<IActionResult> SavePlan([FromBody] Plan plan)
        {
            var userId = User.GetUserId(_env.IsDevelopment());
            if (userId is null)
            {
                return Unauthorized();
            }
            plan.UserId = userId.Value;
            await _planService.SavePlanAsync(plan);
            return NoContent();
        }
    }
}
