using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DropWeightBackend.API.DTOs;
using DropWeightBackend.Domain.Entities;
using DropWeightBackend.Api.Services.Interfaces;

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

        // Get all schedules for a specific user
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetSchedulesForUser(int userId)
        {
            var schedules = await _workoutScheduleService.GetSchedulesForUserAsync(userId);
            if (schedules == null || !schedules.Any())
            {
                return NotFound("No workout schedules found for this user.");
            }

            var response = schedules.Select(s => new WorkoutScheduleResponse
            {
                WorkoutScheduleId = s.WorkoutScheduleId,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                WorkoutId = s.WorkoutId,
                WorkoutType = s.Workout?.Type.ToString(), // Assuming Workout.Type is an Enum
                Reps = s.Workout?.Reps ?? 0, // Workout details
                UserId = s.UserId
            });

            return Ok(response);
        }

        // Get a specific workout schedule by ID
        [HttpGet("{id}")]
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
                WorkoutId = schedule.WorkoutId,
                WorkoutType = schedule.Workout?.Type.ToString(),
                Reps = schedule.Workout?.Reps ?? 0,
                UserId = schedule.UserId
            };

            return Ok(response);
        }

        // Create a new workout schedule
        [HttpPost]
        public async Task<IActionResult> AddSchedule([FromBody] WorkoutScheduleRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var workoutSchedule = new WorkoutSchedule
            {
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                WorkoutId = request.WorkoutId,
                UserId = request.UserId
            };

            await _workoutScheduleService.AddScheduleAsync(workoutSchedule);

            var response = new WorkoutScheduleResponse
            {
                WorkoutScheduleId = workoutSchedule.WorkoutScheduleId,
                StartTime = workoutSchedule.StartTime,
                EndTime = workoutSchedule.EndTime,
                WorkoutId = workoutSchedule.WorkoutId,
                WorkoutType = null, // Workout details will be fetched separately
                UserId = workoutSchedule.UserId
            };

            return CreatedAtAction(nameof(GetScheduleById), new { id = workoutSchedule.WorkoutScheduleId }, response);
        }

        // Update an existing workout schedule
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSchedule(int id, [FromBody] WorkoutScheduleRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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

        // Delete a workout schedule by ID
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
