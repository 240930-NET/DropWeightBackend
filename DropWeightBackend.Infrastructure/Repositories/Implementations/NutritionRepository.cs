using Microsoft.EntityFrameworkCore;
using DropWeightBackend.Domain.Entities;
using DropWeightBackend.Infrastructure.Data;
using DropWeightBackend.Infrastructure.Repositories.Interfaces;

namespace DropWeightBackend.Infrastructure.Repositories.Implementations
{
    public class NutritionRepository : INutritionRepository
    {
        private readonly DropWeightContext _context;

        public NutritionRepository(DropWeightContext context)
        {
            _context = context;
        }

        public async Task<Nutrition?> GetNutritionByIdAsync(int nutritionId)
        {
            return await _context.Nutritions
                .FirstOrDefaultAsync(n => n.NutritionId == nutritionId);
        }

        public async Task<IEnumerable<Nutrition>> GetAllNutritionsAsync()
        {
            return await _context.Nutritions.ToListAsync();
        }

        public async Task<IEnumerable<Nutrition>> GetNutritionsByUserIdAsync(int userId)
        {
            return await _context.Nutritions
                .Where(n => n.UserId == userId)
                .ToListAsync();
        }

        public async Task AddNutritionAsync(Nutrition nutrition)
        {
            await _context.Nutritions.AddAsync(nutrition);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateNutritionAsync(Nutrition nutrition)
        {
            _context.Nutritions.Update(nutrition);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteNutritionAsync(int nutritionId)
        {
            var nutrition = await GetNutritionByIdAsync(nutritionId);
            if (nutrition != null)
            {
                _context.Nutritions.Remove(nutrition);
                await _context.SaveChangesAsync();
            }
        }
    }
}