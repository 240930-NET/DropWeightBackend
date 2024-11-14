using DropWeightBackend.Domain.Entities;
using DropWeightBackend.Infrastructure.Repositories.Interfaces;

namespace DropWeightBackend.Api.Services {
    public class NutritionService : INutritionService {

        private readonly INutritionRepository _nutritionRepo;
        public NutritionService (INutritionRepository nutritionRepo) {
            _nutritionRepo = nutritionRepo;
        }

        public async Task<Nutrition> GetNutritionById(int nutritionId) {
            Nutrition nutrition = await _nutritionRepo.GetNutritionByIdAsync(nutritionId) 
                            ?? throw new Exception("A nutrition with this id does not exist");
            return nutrition;
        }
        public async Task<IEnumerable<Nutrition>> GetAllNutritionsAsync() {
            return await _nutritionRepo.GetAllNutritionsAsync();
        }
        public async Task AddNutritionAsync(Nutrition nutrition) {
            await _nutritionRepo.AddNutritionAsync(nutrition);
        }
        public async Task UpdateNutritionAsync(Nutrition nutrition) {
            await _nutritionRepo.UpdateNutritionAsync(nutrition);
        }
        public async Task DeleteNutritionAsync(int nutritionId) {
            await _nutritionRepo.DeleteNutritionAsync(nutritionId);
        }
    }
}