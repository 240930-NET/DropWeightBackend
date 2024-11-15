using System.Collections.Generic;
using System.Threading.Tasks;
using DropWeightBackend.Domain.Entities;
using DropWeightBackend.Domain.Enums;
using DropWeightBackend.Infrastructure.Repositories.Interfaces;
using DropWeightBackend.Api.DTOs;

namespace DropWeightBackend.Api.Services
{
    public class WorkoutService : IWorkoutService
    {
        private readonly IWorkoutRepository _workoutRepository;

        public WorkoutService(IWorkoutRepository workoutRepository)
        {
            _workoutRepository = workoutRepository;
        }

        public async Task<WorkoutDto> GetWorkoutByIdAsync(int workoutId)
        {
            var workout = await _workoutRepository.GetWorkoutByIdAsync(workoutId);
            return workout == null ? null : MapToDto(workout);
        }

        public async Task<IEnumerable<WorkoutDto>> GetAllWorkoutsAsync()
        {
            var workouts = await _workoutRepository.GetAllWorkoutsAsync();
            return workouts.Select(MapToDto);
        }

        public async Task<IEnumerable<WorkoutDto>> GetWorkoutsByTypeAsync(WorkoutType type)
        {
            var workouts = await _workoutRepository.GetWorkoutsByTypeAsync(type);
            return workouts.Select(MapToDto);
        }

        public async Task AddWorkoutAsync(CreateWorkoutDto dto)
        {
            var workout = new Workout
            {
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Type = dto.Type,
                Reps = dto.Reps
            };
            await _workoutRepository.AddWorkoutAsync(workout);
        }

        public async Task UpdateWorkoutAsync(UpdateWorkoutDto dto)
        {
            var workout = await _workoutRepository.GetWorkoutByIdAsync(dto.WorkoutId);
            if (workout == null) return;

            workout.StartTime = dto.StartTime;
            workout.EndTime = dto.EndTime;
            workout.Type = dto.Type;
            workout.Reps = dto.Reps;
            await _workoutRepository.UpdateWorkoutAsync(workout);
        }

        public async Task DeleteWorkoutAsync(int workoutId)
        {
            await _workoutRepository.DeleteWorkoutAsync(workoutId);
        }

        private WorkoutDto MapToDto(Workout workout)
        {
            return new WorkoutDto
            {
                WorkoutId = workout.WorkoutId,
                StartTime = workout.StartTime,
                EndTime = workout.EndTime,
                Type = workout.Type,
                Reps = workout.Reps,
                GeoSpatials = workout.GeoSpatials?.Select(g => new GeoSpatialDto
                {
                    GeoSpatialId = g.GeoSpatialId,
                    Latitude = g.Latitude,
                    Longitude = g.Longitude
                }).ToList()
            };
        }
    }
}
