

using DropWeightBackend.Domain.Entities;

namespace DropWeightBackend.Api.Services {
    public interface INutritionService {
        Task<Nutrition> GetNutritionById(int nutritionId);
        Task<IEnumerable<Nutrition>> GetAllNutritionsAsync();
        Task AddNutritionAsync(Nutrition nutrition);
        Task UpdateNutritionAsync(Nutrition nutrition);
        Task DeleteNutritionAsync(int nutritionId);
    }
}