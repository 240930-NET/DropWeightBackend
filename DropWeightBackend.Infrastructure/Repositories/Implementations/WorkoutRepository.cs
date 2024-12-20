using Microsoft.EntityFrameworkCore;
using DropWeightBackend.Domain.Entities;
using DropWeightBackend.Infrastructure.Data;
using DropWeightBackend.Domain.Enums;
using DropWeightBackend.Infrastructure.Repositories.Interfaces;

namespace DropWeightBackend.Infrastructure.Repositories.Implementations
{
    public class WorkoutRepository : IWorkoutRepository
    {
        private readonly DropWeightContext _context;

        public WorkoutRepository(DropWeightContext context)
        {
            _context = context;
        }

        public async Task<Workout?> GetWorkoutByIdAsync(int workoutId)
        {
            return await _context.Workouts
                .Include(w => w.User)
                .Include(w => w.GeoSpatials)
                .FirstOrDefaultAsync(w => w.WorkoutId == workoutId);
        }

        public async Task<IEnumerable<Workout>> GetAllWorkoutsAsync()
        {
            return await _context.Workouts
                .Include(w => w.User)
                .Include(w => w.GeoSpatials)
                .ToListAsync();
        }

        public async Task<IEnumerable<Workout>> GetWorkoutsByTypeAsync(WorkoutType type)
        {
            return await _context.Workouts
                .Include(w => w.User)
                .Include(w => w.GeoSpatials)
                .Where(w => w.Type == type)
                .ToListAsync();
        }

        public async Task AddWorkoutAsync(Workout workout)
        {
            await _context.Workouts.AddAsync(workout);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateWorkoutAsync(Workout workout)
        {
            _context.Workouts.Update(workout);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteWorkoutAsync(int workoutId)
        {
            var workout = await GetWorkoutByIdAsync(workoutId);
            if (workout != null)
            {
                _context.Workouts.Remove(workout);
                await _context.SaveChangesAsync();
            }
        }
    }
}
