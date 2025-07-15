using FitnessTracker.API.Models;

namespace FitnessTracker.API.Services;

public interface INutritionAggregatorService
{
    Task<FoodEntry> GetMacrosForBarcodeAsync(string barcode, Guid userId);
    Task<FoodEntry> CreateManualEntryAsync(double protein, double carbs, double fat, Guid userId);
}
