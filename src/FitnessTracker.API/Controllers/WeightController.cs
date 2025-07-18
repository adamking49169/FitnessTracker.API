using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FitnessTracker.Core.Models;
using FitnessTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WeightController : ControllerBase
{
    private readonly FitnessTrackerDbContext _db;

    public WeightController(FitnessTrackerDbContext db) => _db = db;

    private bool TryGetUserId(out Guid userId)
    {
        var claim = User.FindFirst("oid");
        if (claim != null && Guid.TryParse(claim.Value, out userId))
        {
            return true;
        }
        userId = default;
        return false;
    }

    [HttpPost]
    public async Task<ActionResult<WeightEntry>> Post([FromBody] WeightEntry input)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }
        input.Id = Guid.NewGuid();
        input.UserId = userId;
        input.Date = DateTime.UtcNow;
        _db.WeightEntries.Add(input);
        await _db.SaveChangesAsync();
        return Ok(input);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WeightEntry>>> GetAll()
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }
        var list = await _db.WeightEntries
            .Where(w => w.UserId == userId)
            .OrderBy(w => w.Date)
            .ToListAsync();
        return Ok(list);
    }
}
