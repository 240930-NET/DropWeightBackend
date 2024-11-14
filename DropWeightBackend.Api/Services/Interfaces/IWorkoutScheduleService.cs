using System.Collections.Generic;
using System.Threading.Tasks;
using DropWeightBackend.Domain.Entities;

namespace DropWeightBackend.Services
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
