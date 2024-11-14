using System.Collections.Generic;
using System.Threading.Tasks;
using DropWeightBackend.Domain.Entities;
using DropWeightBackend.Domain.Enums;
using DropWeightBackend.Infrastructure.Repositories.Interfaces;

namespace DropWeightBackend.Api.Services
{
    public class WorkoutService : IWorkoutService
    {
        private readonly IWorkoutRepository _workoutRepository;

        public WorkoutService(IWorkoutRepository workoutRepository)
        {
            _workoutRepository = workoutRepository;
        }

        public async Task<Workout> GetWorkoutByIdAsync(int workoutId)
        {
            return await _workoutRepository.GetWorkoutByIdAsync(workoutId);
        }

        public async Task<IEnumerable<Workout>> GetAllWorkoutsAsync()
        {
            return await _workoutRepository.GetAllWorkoutsAsync();
        }

        public async Task<IEnumerable<Workout>> GetWorkoutsByTypeAsync(WorkoutType type)
        {
            return await _workoutRepository.GetWorkoutsByTypeAsync(type);
        }

        public async Task AddWorkoutAsync(Workout workout)
        {
            await _workoutRepository.AddWorkoutAsync(workout);
        }

        public async Task UpdateWorkoutAsync(Workout workout)
        {
            await _workoutRepository.UpdateWorkoutAsync(workout);
        }

        public async Task DeleteWorkoutAsync(int workoutId)
        {
            await _workoutRepository.DeleteWorkoutAsync(workoutId);
        }
    }
}
