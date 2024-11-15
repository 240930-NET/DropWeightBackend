using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DropWeightBackend.API.DTOs.WorkoutSchedule;
using DropWeightBackend.Domain.Entities;
using DropWeightBackend.Api.Services;

namespace DropWeightBackend.API.Controllers
{
    [ApiController]
    [Route("api/workout-schedules")]
    public class WorkoutScheduleController : ControllerBase
    {
        private readonly IWorkoutScheduleService _workoutScheduleService;

        public WorkoutScheduleController(IWorkoutScheduleService workoutScheduleService)
        {
            _workoutScheduleService = workoutScheduleService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetSchedulesForUser(int userId)
        {
            var schedules = await _workoutScheduleService.GetSchedulesForUserAsync(userId);
            var response = schedules.Select(s => new WorkoutScheduleResponse
            {
                WorkoutScheduleId = s.WorkoutScheduleId,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                WorkoutName = s.Workout?.Name, 
                UserId = s.UserId
            });

            return Ok(response);
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetScheduleById(int id)
        {
            var schedule = await _workoutScheduleService.GetScheduleByIdAsync(id);
            if (schedule == null)
            {
                return NotFound("Workout schedule not found.");
            }

            var response = new WorkoutScheduleResponse
            {
                WorkoutScheduleId = schedule.WorkoutScheduleId,
                StartTime = schedule.StartTime,
                EndTime = schedule.EndTime,
                WorkoutName = schedule.Workout?.Name,
                UserId = schedule.UserId
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddSchedule([FromBody] WorkoutScheduleRequest request)
        {
            var workoutSchedule = new WorkoutSchedule
            {
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                WorkoutId = request.WorkoutId,
                UserId = request.UserId
            };

            await _workoutScheduleService.AddScheduleAsync(workoutSchedule);
            return CreatedAtAction(nameof(GetScheduleById), new { id = workoutSchedule.WorkoutScheduleId }, workoutSchedule);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSchedule(int id, [FromBody] WorkoutScheduleRequest request)
        {
            var existingSchedule = await _workoutScheduleService.GetScheduleByIdAsync(id);
            if (existingSchedule == null)
            {
                return NotFound("Workout schedule not found.");
            }

            existingSchedule.StartTime = request.StartTime;
            existingSchedule.EndTime = request.EndTime;
            existingSchedule.WorkoutId = request.WorkoutId;
            existingSchedule.UserId = request.UserId;

            await _workoutScheduleService.UpdateScheduleAsync(existingSchedule);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            var schedule = await _workoutScheduleService.GetScheduleByIdAsync(id);
            if (schedule == null)
            {
                return NotFound("Workout schedule not found.");
            }

            await _workoutScheduleService.DeleteScheduleAsync(id);
            return NoContent();
        }
    }
}
