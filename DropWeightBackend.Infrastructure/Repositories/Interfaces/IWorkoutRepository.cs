using System.Collections.Generic;
using System.Threading.Tasks;
using DropWeight.Domain.Entities;
using GreaterGradesBackend.Domain.Enums;

namespace DropWeight.Domain.Repositories
{
    public interface IWorkoutRepository
    {
        Task<Workout> GetWorkoutByIdAsync(int workoutId);
        Task<IEnumerable<Workout>> GetAllWorkoutsAsync();
        Task<IEnumerable<Workout>> GetWorkoutsByTypeAsync(WorkoutType type);
        Task AddWorkoutAsync(Workout workout);
        Task UpdateWorkoutAsync(Workout workout);
        Task DeleteWorkoutAsync(int workoutId);
    }
}
