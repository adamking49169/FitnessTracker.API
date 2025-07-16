using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FitnessTracker.API.Models;
using FitnessTracker.API.Services;

namespace FitnessTracker.API.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MealPlanController : ControllerBase
{
private string test = "This is a test for the MealPlanController.";
    
    private readonly IOpenAiMealPlanService _svc;
    
    public MealPlanController(IOpenAiMealPlanService svc) => _svc = svc;

    [HttpPost("generate/{calories}")]
    public async Task<ActionResult<MealPlan>> Generate(int calories)
    {
        var userId = Guid.Parse(User.FindFirst("oid")!.Value);
        var plan = await _svc.GenerateDailyMealPlanAsync(calories, userId);
        return Ok(plan);
    }
}
