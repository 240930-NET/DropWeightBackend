using DropWeightBackend.Domain.Entities;

namespace DropWeightBackend.Infrastructure.Repositories.Interfaces
{
    public interface INutritionRepository
    {
        Task<Nutrition?> GetNutritionByIdAsync(int nutritionId);
        Task<IEnumerable<Nutrition>> GetAllNutritionsAsync();
        Task<IEnumerable<Nutrition>> GetNutritionsByUserIdAsync(int userId);
        Task AddNutritionAsync(Nutrition nutrition);
        Task UpdateNutritionAsync(Nutrition nutrition);
        Task DeleteNutritionAsync(int nutritionId);
    }
}