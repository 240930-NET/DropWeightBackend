using Microsoft.EntityFrameworkCore;
using DropWeightBackend.Domain.Entities;
using DropWeightBackend.Infrastructure.Data;
using DropWeightBackend.Infrastructure.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DropWeightBackend.Infrastructure.Repositories.Implementations
{
    public class WorkoutScheduleRepository : IWorkoutScheduleRepository
    {
        private readonly DropWeightContext _context;

        public WorkoutScheduleRepository(DropWeightContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WorkoutSchedule>> GetSchedulesForUserAsync(int userId)
        {
            return await _context.WorkoutSchedules
                .Include(ws => ws.Workout)
                .Where(ws => ws.UserId == userId)
                .ToListAsync();
        }

        public async Task<WorkoutSchedule?> GetScheduleByIdAsync(int scheduleId)
        {
            return await _context.WorkoutSchedules
                .Include(ws => ws.Workout) 
                .FirstOrDefaultAsync(ws => ws.WorkoutScheduleId == scheduleId);
        }

        public async Task AddScheduleAsync(WorkoutSchedule workoutSchedule)
        {
            await _context.WorkoutSchedules.AddAsync(workoutSchedule);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateScheduleAsync(WorkoutSchedule workoutSchedule)
        {
            _context.WorkoutSchedules.Update(workoutSchedule);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteScheduleAsync(int scheduleId)
        {
            var schedule = await GetScheduleByIdAsync(scheduleId);
            if (schedule != null)
            {
                _context.WorkoutSchedules.Remove(schedule);
                await _context.SaveChangesAsync();
            }
        }
    }
}
