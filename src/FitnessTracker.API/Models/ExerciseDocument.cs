
namespace FitnessTracker.API.Models;

public class ExerciseDocument
{
    public string id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
    public int CaloriesPerMinute { get; set; }
}
