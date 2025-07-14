namespace FitnessTracker.Core.Models
{
    public class WorkoutLog
    {
        public Guid WorkoutLogId { get; set; }
        public Guid UserId { get; set; }
        public DateOnly Date { get; set; }
        public string Type { get; set; } = "";
        public string? ExerciseName { get; set; }
        public int? DurationMinutes { get; set; }
        public int CaloriesBurned { get; set; }
        public int? Reps { get; set; }
        public int? Sets { get; set; }
        public decimal? Weight { get; set; }
    }
}
