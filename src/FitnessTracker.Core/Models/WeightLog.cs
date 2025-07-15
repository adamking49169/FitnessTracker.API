namespace FitnessTracker.Core.Models
{
    public class WeightLog
    {
        public Guid WeightLogId { get; set; }
        public Guid UserId { get; set; }
        public DateOnly Date { get; set; }
        public decimal Weight { get; set; }
    }
}
