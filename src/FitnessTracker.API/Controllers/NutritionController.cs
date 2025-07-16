using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FitnessTracker.Core.Models;
using FitnessTracker.Infrastructure.Services;

namespace FitnessTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NutritionController : ControllerBase
{
    private readonly INutritionAggregatorService _svc;

    public NutritionController(INutritionAggregatorService svc) => _svc = svc;

    [HttpGet("barcode/{upc}")]
    public async Task<ActionResult<FoodEntry>> GetByBarcode(string upc)
    {
        var userId = Guid.Parse(User.FindFirst("oid")!.Value);
        var entry = await _svc.GetMacrosForBarcodeAsync(upc, userId);
        return Ok(entry);
    }

    [HttpPost("manual")]
    public async Task<ActionResult<FoodEntry>> PostManual([FromBody] FoodEntry input)
    {
        var userId = Guid.Parse(User.FindFirst("oid")!.Value);
        var entry = await _svc.CreateManualEntryAsync(input.Protein, input.Carbs, input.Fat, userId);
        return Ok(entry);
    }
}
