namespace FitnessTracker.Core.Models
{
    public class MealLog
    {
        public Guid MealLogId { get; set; }
        public Guid UserId { get; set; }
        public DateOnly Date { get; set; }
        public int Calories { get; set; }
        public decimal Protein { get; set; }
        public decimal Carbs { get; set; }
        public decimal Fats { get; set; }
        public string? Source { get; set; }
    }
}
