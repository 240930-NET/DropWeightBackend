using DropWeightBackend.Domain.Entities;
using DropWeightBackend.Infrastructure.UnitOfWork;
using DropWeightBackend.Api.Services.Interfaces;

namespace DropWeightBackend.Api.Services.Implementations
{
    public class NutritionService : INutritionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NutritionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Nutrition?> GetNutritionByIdAsync(int nutritionId)
        {
            Nutrition? nutrition = await _unitOfWork.Nutritions.GetNutritionByIdAsync(nutritionId);
            return nutrition;
        }

        public async Task<IEnumerable<Nutrition>> GetAllNutritionsAsync()
        {
            return await _unitOfWork.Nutritions.GetAllNutritionsAsync();
        }

        public async Task<IEnumerable<Nutrition>> GetNutritionsByUserIdAsync(int userId) {
            return await _unitOfWork.Nutritions.GetNutritionsByUserIdAsync(userId);
        }

        public async Task AddNutritionAsync(Nutrition nutrition)
        {
            ArgumentNullException.ThrowIfNull(nutrition);
            await _unitOfWork.Nutritions.AddNutritionAsync(nutrition);
            await _unitOfWork.CompleteAsync(); // Save changes
        }

        public async Task UpdateNutritionAsync(Nutrition nutrition)
        {
            ArgumentNullException.ThrowIfNull(nutrition);

            await _unitOfWork.Nutritions.UpdateNutritionAsync(nutrition);
            await _unitOfWork.CompleteAsync(); // Save changes
        }

        public async Task DeleteNutritionAsync(int nutritionId)
        {
            await _unitOfWork.Nutritions.DeleteNutritionAsync(nutritionId);
            await _unitOfWork.CompleteAsync(); // Save changes
        }
    }
}
