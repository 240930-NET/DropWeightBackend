using DropWeightBackend.Domain.Entities;

namespace DropWeightBackend.Api.Services.Interfaces
{
    public interface IWorkoutScheduleService
    {
        Task<IEnumerable<WorkoutSchedule>> GetSchedulesForUserAsync(int userId);
        Task<WorkoutSchedule?> GetScheduleByIdAsync(int scheduleId);
        Task AddScheduleAsync(WorkoutSchedule workoutSchedule);
        Task UpdateScheduleAsync(WorkoutSchedule workoutSchedule);
        Task DeleteScheduleAsync(int scheduleId);
    }
}
