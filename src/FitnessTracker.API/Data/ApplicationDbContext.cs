using FitnessTracker.Core.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using FitnessTracker.API.Models;
using User = FitnessTracker.API.Models.User;

namespace FitnessTracker.API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opts)
        : base(opts) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<FoodEntry> FoodEntries => Set<FoodEntry>();
    public DbSet<WeightEntry> WeightEntries => Set<WeightEntry>();
    public DbSet<MealPlan> MealPlans => Set<MealPlan>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasKey(u => u.Id);
        modelBuilder.Entity<FoodEntry>().HasKey(f => f.Id);
        modelBuilder.Entity<WeightEntry>().HasKey(w => w.Id);
        modelBuilder.Entity<MealPlan>().HasKey(m => m.Id);
        // additional indexes and relationships here
    }
}
