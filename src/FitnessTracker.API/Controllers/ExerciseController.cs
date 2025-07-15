using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FitnessTracker.API.Models;
using FitnessTracker.API.Services;

namespace FitnessTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ExerciseController : ControllerBase
{
    private readonly ICosmosExerciseService _svc;

    public ExerciseController(ICosmosExerciseService svc) => _svc = svc;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExerciseDocument>>> GetAll()
        => Ok(await _svc.GetAllAsync());
}
