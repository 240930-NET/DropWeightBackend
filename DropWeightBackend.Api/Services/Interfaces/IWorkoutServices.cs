using System.Collections.Generic;
using System.Threading.Tasks;
using DropWeightBackend.Domain.Entities;
using DropWeightBackend.Domain.Enums;

namespace DropWeightBackend.Api.Services
{
    public interface IWorkoutService
    {
        Task<Workout> GetWorkoutByIdAsync(int workoutId);
        Task<IEnumerable<Workout>> GetAllWorkoutsAsync();
        Task<IEnumerable<Workout>> GetWorkoutsByTypeAsync(WorkoutType type);
        Task AddWorkoutAsync(Workout workout);
        Task UpdateWorkoutAsync(Workout workout);
        Task DeleteWorkoutAsync(int workoutId);
    }
}
