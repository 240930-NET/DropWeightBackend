using DropWeightBackend.Domain.Entities;
using DropWeightBackend.Domain.Enums;
using DropWeightBackend.Api.DTOs;

namespace DropWeightBackend.Api.Services.Interfaces
{
    public interface IWorkoutService
    {
        Task<WorkoutDto> GetWorkoutByIdAsync(int workoutId);
        Task<IEnumerable<WorkoutDto>> GetAllWorkoutsAsync();
        Task<IEnumerable<WorkoutDto>> GetWorkoutsByTypeAsync(WorkoutType type);
        Task AddWorkoutAsync(CreateWorkoutDto dto);
        Task UpdateWorkoutAsync(UpdateWorkoutDto dto);
        Task DeleteWorkoutAsync(int workoutId);
    }
}

