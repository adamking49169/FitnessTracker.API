namespace FitnessTracker.Core.Models
{
    public class Plan
    {
        public Guid PlanId { get; set; }
        public Guid UserId { get; set; }
        public decimal? StartWeight { get; set; }
        public decimal? TargetWeight { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public int DailyCalorieTarget { get; set; }
    }
}
