using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.API.Models;

public class WeightEntry
{
    [Key]
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public double WeightKg { get; set; }

    public User? User { get; set; }
}
