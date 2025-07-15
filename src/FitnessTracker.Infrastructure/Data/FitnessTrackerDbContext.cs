using FitnessTracker.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessTracker.Infrastructure.Data
{
    public class FitnessTrackerDbContext : DbContext
    {
        public FitnessTrackerDbContext(DbContextOptions<FitnessTrackerDbContext> options)
            : base(options)
        {
        }

        public DbSet<Plan> Plans => Set<Plan>();
        public DbSet<WorkoutLog> WorkoutLogs => Set<WorkoutLog>();
        public DbSet<WeightLog> WeightLogs => Set<WeightLog>();
        public DbSet<MealLog> MealLogs => Set<MealLog>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var dateOnlyConverter = new DateOnlyConverter();

            modelBuilder.Entity<Plan>(ConfigurePlan);
            modelBuilder.Entity<WorkoutLog>(e =>
            {
                e.Property(w => w.Date).HasConversion(dateOnlyConverter);
                e.Property(w => w.Weight).HasColumnType("decimal(18,2)");
            });
            modelBuilder.Entity<WeightLog>(e =>
            {
                e.Property(w => w.Date).HasConversion(dateOnlyConverter);
                e.Property(w => w.Weight).HasColumnType("decimal(18,2)");
            });
            modelBuilder.Entity<MealLog>(e =>
            {
                e.Property(m => m.Date).HasConversion(dateOnlyConverter);
                e.Property(m => m.Protein).HasColumnType("decimal(18,2)");
                e.Property(m => m.Carbs).HasColumnType("decimal(18,2)");
                e.Property(m => m.Fats).HasColumnType("decimal(18,2)");
            });

            void ConfigurePlan(EntityTypeBuilder<Plan> builder)
            {
                builder.Property(p => p.StartDate).HasConversion(dateOnlyConverter);
                builder.Property(p => p.EndDate).HasConversion(dateOnlyConverter);
                builder.Property(p => p.StartWeight).HasColumnType("decimal(18,2)");
                builder.Property(p => p.TargetWeight).HasColumnType("decimal(18,2)");
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}