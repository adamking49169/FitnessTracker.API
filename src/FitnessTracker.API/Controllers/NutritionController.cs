using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using FitnessTracker.Core.Models;
using FitnessTracker.Infrastructure.Services;

namespace FitnessTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NutritionController : ControllerBase
{
    private readonly INutritionAggregatorService _svc;
    private readonly IWebHostEnvironment _env;

    public NutritionController(INutritionAggregatorService svc, IWebHostEnvironment env)
    {
        _svc = svc;
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

    [HttpGet("barcode/{upc}")]
    public async Task<ActionResult<FoodEntry>> GetByBarcode(string upc)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }
        var entry = await _svc.GetMacrosForBarcodeAsync(upc, userId);
        return Ok(entry);
    }

    [HttpPost("manual")]
    public async Task<ActionResult<FoodEntry>> PostManual([FromBody] FoodEntry input)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }
        var entry = await _svc.CreateManualEntryAsync(input.Protein, input.Carbs, input.Fat, userId);
        return Ok(entry);
    }
}
