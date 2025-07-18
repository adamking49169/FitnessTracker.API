using Moq;
using FitnessTracker.API.Controllers;
using FitnessTracker.Core.Models;
using FitnessTracker.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace FitnessTracker.Tests;

public class WeightControllerTests
{
    private static WeightController CreateController(FitnessTrackerDbContext ctx)
    {
        var env = Mock.Of<IWebHostEnvironment>(e => e.EnvironmentName == "Development");
        var ctrl = new WeightController(ctx, env);
        ctrl.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = new System.Security.Claims.ClaimsPrincipal(new System.Security.Claims.ClaimsIdentity())
        };
        return ctrl;
    }

    private static FitnessTrackerDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<FitnessTrackerDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new FitnessTrackerDbContext(options);
    }

    [Fact]
    public async Task Post_saves_entry()
    {
        using var db = CreateDb();
        var ctrl = CreateController(db);
        var entry = new WeightEntry { WeightKg = 80 };

        var result = await ctrl.Post(entry);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var saved = await db.WeightEntries.FirstAsync();
        Assert.Equal(saved.Id, ((WeightEntry)ok.Value!).Id);
    }

    [Fact]
    public async Task GetAll_returns_user_entries()
    {
        using var db = CreateDb();
        db.WeightEntries.Add(new WeightEntry { Id = Guid.NewGuid(), UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"), Date = DateTime.UtcNow, WeightKg = 70 });
        db.WeightEntries.Add(new WeightEntry { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Date = DateTime.UtcNow, WeightKg = 90 });
        await db.SaveChangesAsync();
        var ctrl = CreateController(db);

        var result = await ctrl.GetAll();

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var list = Assert.IsAssignableFrom<IEnumerable<WeightEntry>>(ok.Value);
        Assert.Single(list);
    }
}
