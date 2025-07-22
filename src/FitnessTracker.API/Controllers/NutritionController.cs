using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using FitnessTracker.Core.Models;
using FitnessTracker.API.Extensions;
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


    [HttpGet("barcode/{upc}")]
    public async Task<ActionResult<FoodEntry>> GetByBarcode(string upc)
    {
        var userId = User.GetUserId(_env.IsDevelopment());
        if (userId is null)
        {
            return Unauthorized();
        }
        var entry = await _svc.GetMacrosForBarcodeAsync(upc, userId.Value);
        return Ok(entry);
    }

    [HttpPost("manual")]
    public async Task<ActionResult<FoodEntry>> PostManual([FromBody] FoodEntry input)
    {
        var userId = User.GetUserId(_env.IsDevelopment());
        if (userId is null)
        {
            return Unauthorized();
        }
        var entry = await _svc.CreateManualEntryAsync(input.Protein, input.Carbs, input.Fat, userId.Value);
        return Ok(entry);
    }
}
