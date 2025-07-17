using FitnessTracker.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.Infrastructure
{
    public interface INutritionService
    {
        Task<FoodEntry> GetMacrosForBarcodeAsync(string barcode, Guid userId);
        Task<FoodEntry> CreateManualEntryAsync(double protein, double carbs, double fat, Guid userId);
    }
}
