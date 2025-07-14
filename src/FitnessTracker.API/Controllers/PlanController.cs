using FitnessTracker.Infrastructure.Services;
using FitnessTracker.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.API.Controllers
{
    [ApiController]
    [Route("user/plan")]
    public class PlanController : ControllerBase
    {
        private readonly IPlanService _service;
        public PlanController(IPlanService service) => _service = service;

        [HttpPost]
        public async Task<ActionResult<Plan>> GeneratePlan([FromBody] PlanRequest request)
        {
            var result = await _service.GeneratePlanAsync(request);
            return Ok(result);
        }
    }
}
