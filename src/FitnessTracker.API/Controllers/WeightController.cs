using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FitnessTracker.API.Data;
using FitnessTracker.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WeightController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public WeightController(ApplicationDbContext db) => _db = db;

    [HttpPost]
    public async Task<ActionResult<WeightEntry>> Post([FromBody] WeightEntry input)
    {
        var userId = Guid.Parse(User.FindFirst("oid")!.Value);
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
        var userId = Guid.Parse(User.FindFirst("oid")!.Value);
        var list = await _db.WeightEntries
            .Where(w => w.UserId == userId)
            .OrderBy(w => w.Date)
            .ToListAsync();
        return Ok(list);
    }
}
