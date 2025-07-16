using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Core.Models;

public class FoodEntry
{
    [Key]
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }

    public double Protein { get; set; }
    public double Carbs { get; set; }
    public double Fat { get; set; }
    public double Calories { get; set; }

}
