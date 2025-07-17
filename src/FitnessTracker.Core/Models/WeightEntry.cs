namespace FitnessTracker.Core.Models;

public class WeightEntry
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public double WeightKg { get; set; }

    public User? User { get; set; }
}

