using DropWeightBackend.Api.Services.Interfaces;
using DropWeightBackend.Domain.Entities;
using DropWeightBackend.Domain.Enums;
using DropWeightBackend.Infrastructure.UnitOfWork;
using DropWeightBackend.Api.DTOs;

namespace DropWeightBackend.Api.Services.Implementations
{
    public class WorkoutService : IWorkoutService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WorkoutService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<WorkoutDto> GetWorkoutByIdAsync(int workoutId)
        {
            var workout = await _unitOfWork.Workouts.GetWorkoutByIdAsync(workoutId);
            return workout == null ? null : MapToDto(workout);
        }

        public async Task<IEnumerable<WorkoutDto>> GetAllWorkoutsAsync()
        {
            var workouts = await _unitOfWork.Workouts.GetAllWorkoutsAsync();
            return workouts.Select(MapToDto);
        }

        public async Task<IEnumerable<WorkoutDto>> GetWorkoutsByTypeAsync(WorkoutType type)
        {
            var workouts = await _unitOfWork.Workouts.GetWorkoutsByTypeAsync(type);
            return workouts.Select(MapToDto);
        }

        public async Task AddWorkoutAsync(CreateWorkoutDto dto)
        {
            var workout = new Workout
            {
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Type = dto.Type,
                Reps = dto.Reps,
                User = await _unitOfWork.Users.GetUserByIdAsync(dto.UserId),
                UserId = dto.UserId
            };
            await _unitOfWork.Workouts.AddWorkoutAsync(workout);
            await _unitOfWork.CompleteAsync(); // Save changes
        }

        public async Task UpdateWorkoutAsync(UpdateWorkoutDto dto)
        {
            var workout = await _unitOfWork.Workouts.GetWorkoutByIdAsync(dto.WorkoutId);
            if (workout == null) return;

            workout.StartTime = dto.StartTime;
            workout.EndTime = dto.EndTime;
            workout.Type = dto.Type;
            workout.Reps = dto.Reps;

            await _unitOfWork.Workouts.UpdateWorkoutAsync(workout);
            await _unitOfWork.CompleteAsync(); // Save changes
        }

        public async Task DeleteWorkoutAsync(int workoutId)
        {
            await _unitOfWork.Workouts.DeleteWorkoutAsync(workoutId);
            await _unitOfWork.CompleteAsync(); // Save changes
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
                UserId = workout.UserId,
                GeoSpatials = workout.GeoSpatials?.Select(g => new GeoSpatialDto
                {
                    Latitude = g.Latitude,
                    Longitude = g.Longitude
                }).ToList()
            };
        }
    }
}
