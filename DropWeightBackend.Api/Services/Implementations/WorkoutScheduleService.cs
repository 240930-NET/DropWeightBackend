using System.Collections.Generic;
using System.Threading.Tasks;
using DropWeightBackend.Domain.Entities;
using DropWeightBackend.Infrastructure.Repositories.Interfaces;

namespace DropWeightBackend.Services
{
    public class WorkoutScheduleService : IWorkoutScheduleService
    {
        private readonly IWorkoutScheduleRepository _workoutScheduleRepository;

        public WorkoutScheduleService(IWorkoutScheduleRepository workoutScheduleRepository)
        {
            _workoutScheduleRepository = workoutScheduleRepository;
        }

        public async Task<IEnumerable<WorkoutSchedule>> GetSchedulesForUserAsync(int userId)
        {
            return await _workoutScheduleRepository.GetSchedulesForUserAsync(userId);
        }

        public async Task<WorkoutSchedule?> GetScheduleByIdAsync(int scheduleId)
        {
            return await _workoutScheduleRepository.GetScheduleByIdAsync(scheduleId);
        }

        public async Task AddScheduleAsync(WorkoutSchedule workoutSchedule)
        {
            await _workoutScheduleRepository.AddScheduleAsync(workoutSchedule);
        }

        public async Task UpdateScheduleAsync(WorkoutSchedule workoutSchedule)
        {
            await _workoutScheduleRepository.UpdateScheduleAsync(workoutSchedule);
        }

        public async Task DeleteScheduleAsync(int scheduleId)
        {
            await _workoutScheduleRepository.DeleteScheduleAsync(scheduleId);
        }
    }
}
