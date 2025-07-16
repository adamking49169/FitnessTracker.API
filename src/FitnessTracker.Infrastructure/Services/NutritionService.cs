using System;
using System.Threading.Tasks;
using FitnessTracker.Core.Models;        // adjust to your Core models namespace
using FitnessTracker.API.Services;      // to call the aggregator
using FitnessTracker.API.Models;

namespace FitnessTracker.Infrastructure.Services
{
    public class NutritionService : INutritionService
    {
        private readonly INutritionAggregatorService _aggregator;

        public NutritionService(INutritionAggregatorService aggregator)
        {
            _aggregator = aggregator;
        }

        public Task<FoodEntry> GetMacrosForBarcodeAsync(string barcode, Guid userId)
            => _aggregator.GetMacrosForBarcodeAsync(barcode, userId);

        public Task<FoodEntry> CreateManualEntryAsync(double protein, double carbs, double fat, Guid userId)
            => _aggregator.CreateManualEntryAsync(protein, carbs, fat, userId);
    }
}
