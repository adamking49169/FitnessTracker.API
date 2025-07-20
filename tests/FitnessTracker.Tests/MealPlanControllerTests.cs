using FitnessTracker.API.Controllers;
using FitnessTracker.API.Services;
using FitnessTracker.Infrastructure.Services;
using FitnessTracker.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;

namespace FitnessTracker.Tests;

public class MealPlanControllerTests
{
    private static MealPlanController CreateController(IOpenAiMealPlanService mealSvc, IPlanService planSvc)
    {
        var env = Mock.Of<IWebHostEnvironment>(e => e.EnvironmentName == "Development");
        var ctrl = new MealPlanController(mealSvc, planSvc, env);
        var ctx = new DefaultHttpContext();
        ctx.User = new System.Security.Claims.ClaimsPrincipal(new System.Security.Claims.ClaimsIdentity());
        ctrl.ControllerContext.HttpContext = ctx;
        return ctrl;
    }

    [Fact]
    public async Task Generate_returns_plan_from_service()
    {
        var plan = new MealPlan { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), TotalCalories = 1000, PlanJson = "{}" };
        var mealSvc = new Mock<IOpenAiMealPlanService>();
        mealSvc.Setup(s => s.GenerateDailyMealPlanAsync(1000, It.IsAny<Guid>()))
               .ReturnsAsync(plan);
        var ctrl = CreateController(mealSvc.Object, Mock.Of<IPlanService>());

        var result = await ctrl.Generate(1000);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(plan, ok.Value);
    }

    [Fact]
    public async Task GetPlan_returns_NotFound_when_null()
    {
        var planSvc = new Mock<IPlanService>();
        planSvc.Setup(s => s.GetPlanForUserAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Plan?)null);
        var ctrl = CreateController(Mock.Of<IOpenAiMealPlanService>(), planSvc.Object);

        var result = await ctrl.GetPlan();

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task SavePlan_passes_user_to_service()
    {
        var planSvc = new Mock<IPlanService>();
        Plan? saved = null;
        planSvc.Setup(s => s.SavePlanAsync(It.IsAny<Plan>()))
                .Callback<Plan>(p => saved = p)
                .Returns(Task.CompletedTask);
        var ctrl = CreateController(Mock.Of<IOpenAiMealPlanService>(), planSvc.Object);

        var input = new Plan { DailyCalorieTarget = 1500 };
        var resp = await ctrl.SavePlan(input);

        Assert.IsType<NoContentResult>(resp);
        Assert.NotNull(saved);
        Assert.Equal(input.DailyCalorieTarget, saved!.DailyCalorieTarget);
        Assert.NotEqual(Guid.Empty, saved.UserId);
    }
}
