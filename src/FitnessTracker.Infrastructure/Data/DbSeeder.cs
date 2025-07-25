using FitnessTracker.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(FitnessTrackerDbContext context)
    {
        if (await context.Users.AnyAsync())
        {
            return;
        }

        var user1 = new User
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            Email = "user1@example.com"
        };

        var user2 = new User
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
            Email = "user2@example.com"
        };

        context.Users.AddRange(user1, user2);

        context.WeightEntries.AddRange(
            new WeightEntry
            {
                Id = Guid.NewGuid(),
                UserId = user1.Id,
                Date = DateTime.UtcNow.AddDays(-3),
                WeightKg = 80
            },
            new WeightEntry
            {
                Id = Guid.NewGuid(),
                UserId = user1.Id,
                Date = DateTime.UtcNow.AddDays(-2),
                WeightKg = 79.5
            }
        );

        context.FoodEntries.AddRange(
            new FoodEntry
            {
                Id = Guid.NewGuid(),
                UserId = user1.Id,
                Date = DateTime.UtcNow.AddDays(-2),
                Protein = 40,
                Carbs = 60,
                Fat = 20,
                Calories = 500
            },
            new FoodEntry
            {
                Id = Guid.NewGuid(),
                UserId = user1.Id,
                Date = DateTime.UtcNow.AddDays(-1),
                Protein = 30,
                Carbs = 50,
                Fat = 15,
                Calories = 400
            }
        );

        context.MealPlans.Add(
            new MealPlan
            {
                Id = Guid.NewGuid(),
                UserId = user1.Id,
                Date = DateTime.UtcNow.Date,
                TotalCalories = 2000,
                PlanJson = "{}"
            });

        context.Plans.Add(
            new Plan
            {
                PlanId = Guid.NewGuid(),
                UserId = user1.Id,
                DailyCalorieTarget = 2000,
                StartWeight = 80,
                TargetWeight = 75,
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-7)),
                EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(7))
            });

        context.WorkoutLogs.Add(
            new WorkoutLog
            {
                WorkoutLogId = Guid.NewGuid(),
                UserId = user1.Id,
                Date = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)),
                Type = "Cardio",
                DurationMinutes = 30,
                CaloriesBurned = 300
            });

        context.WeightLogs.Add(
            new WeightLog
            {
                WeightLogId = Guid.NewGuid(),
                UserId = user1.Id,
                Date = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-2)),
                Weight = 80
            });

        await context.SaveChangesAsync();
    }
}
