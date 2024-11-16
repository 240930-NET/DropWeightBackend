using DropWeightBackend.Domain.Entities;

namespace DropWeightBackend.Api.Services.Interfaces {
    public interface INutritionService {
        Task<Nutrition?> GetNutritionByIdAsync(int nutritionId);
        Task<IEnumerable<Nutrition>> GetAllNutritionsAsync();
        Task AddNutritionAsync(Nutrition nutrition);
        Task UpdateNutritionAsync(Nutrition nutrition);
        Task DeleteNutritionAsync(int nutritionId);
    }
}