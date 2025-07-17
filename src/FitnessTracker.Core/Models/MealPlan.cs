namespace FitnessTracker.Core.Models;

public class MealPlan
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public int TotalCalories { get; set; }

    // Stored as JSON
    public string PlanJson { get; set; } = null!;
}

