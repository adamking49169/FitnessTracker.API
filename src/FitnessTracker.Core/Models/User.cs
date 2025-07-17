namespace FitnessTracker.Core.Models;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

