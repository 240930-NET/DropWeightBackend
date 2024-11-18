using DropWeightBackend.Domain.Entities;
using DropWeightBackend.Infrastructure.UnitOfWork;
using DropWeightBackend.Api.Services.Interfaces;

namespace DropWeightBackend.Api.Services.Implementations
{
    public class WorkoutScheduleService : IWorkoutScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WorkoutScheduleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<WorkoutSchedule>> GetSchedulesForUserAsync(int userId)
        {
            return await _unitOfWork.WorkoutSchedules.GetSchedulesForUserAsync(userId);
        }

        public async Task<WorkoutSchedule?> GetScheduleByIdAsync(int scheduleId)
        {
            return await _unitOfWork.WorkoutSchedules.GetScheduleByIdAsync(scheduleId);
        }

        public async Task AddScheduleAsync(WorkoutSchedule workoutSchedule)
        {
            await _unitOfWork.WorkoutSchedules.AddScheduleAsync(workoutSchedule);
            await _unitOfWork.CompleteAsync(); // Save changes
        }

        public async Task UpdateScheduleAsync(WorkoutSchedule workoutSchedule)
        {
            await _unitOfWork.WorkoutSchedules.UpdateScheduleAsync(workoutSchedule);
            await _unitOfWork.CompleteAsync(); // Save changes
        }

        public async Task DeleteScheduleAsync(int scheduleId)
        {
            await _unitOfWork.WorkoutSchedules.DeleteScheduleAsync(scheduleId);
            await _unitOfWork.CompleteAsync(); // Save changes
        }
    }
}
