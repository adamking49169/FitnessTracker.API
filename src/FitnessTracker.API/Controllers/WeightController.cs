using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FitnessTracker.Core.Models;
using FitnessTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using FitnessTracker.API.Extensions;

namespace FitnessTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WeightController : ControllerBase
{
    private readonly FitnessTrackerDbContext _db;
    private readonly IWebHostEnvironment _env;

    public WeightController(FitnessTrackerDbContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }


    [HttpPost]
    public async Task<ActionResult<WeightEntry>> Post([FromBody] WeightEntry input)
    {
        var userId = User.GetUserId(_env.IsDevelopment());
        if (userId is null)
        {
            return Unauthorized();
        }
        input.Id = Guid.NewGuid();
        input.UserId = userId.Value;
        input.Date = DateTime.UtcNow;
        _db.WeightEntries.Add(input);
        await _db.SaveChangesAsync();
        return Ok(input);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WeightEntry>>> GetAll()
    {
        var userId = User.GetUserId(_env.IsDevelopment());
        if (userId is null)
        {
            return Unauthorized();
        }
        var list = await _db.WeightEntries
            .Where(w => w.UserId == userId.Value)
            .OrderBy(w => w.Date)
            .ToListAsync();
        return Ok(list);
    }
}
