using FitnessTracker.Core.Models;
using FitnessTracker.Infrastructure.Data;
using FitnessTracker.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Tests;

public class PlanServiceTests
{
    private static FitnessTrackerDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<FitnessTrackerDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new FitnessTrackerDbContext(options);
    }

    [Fact]
    public async Task GeneratePlan_creates_and_saves_plan()
    {
        using var db = CreateDb();
        var svc = new PlanService(db);
        var req = new PlanRequest { Calories = 2000, UserId = Guid.NewGuid() };

        var plan = await svc.GeneratePlanAsync(req);

        Assert.NotEqual(Guid.Empty, plan.PlanId);
        Assert.Equal(req.UserId, plan.UserId);
        Assert.Equal(req.Calories, plan.DailyCalorieTarget);
        Assert.Equal(1, await db.Plans.CountAsync());
    }

    [Fact]
    public async Task SavePlan_updates_existing()
    {
        using var db = CreateDb();
        var svc = new PlanService(db);
        var plan = new Plan { PlanId = Guid.NewGuid(), UserId = Guid.NewGuid(), DailyCalorieTarget = 1500 };
        db.Plans.Add(plan);
        await db.SaveChangesAsync();

        plan.DailyCalorieTarget = 1800;
        await svc.SavePlanAsync(plan);

        var fromDb = await db.Plans.FirstAsync();
        Assert.Equal(1800, fromDb.DailyCalorieTarget);
    }
}
