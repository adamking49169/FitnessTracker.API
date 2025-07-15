using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.API.Models;

public class User
{
    [Key]
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
