using System.Collections.Generic;
using System.Threading.Tasks;
using DropWeightBackend.Api.DTOs;
using DropWeightBackend.Domain.Enums;
using DropWeightBackend.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DropWeightBackend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkoutController : ControllerBase
    {
        private readonly IWorkoutService _workoutService;

        public WorkoutController(IWorkoutService workoutService)
        {
            _workoutService = workoutService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WorkoutDto>> GetWorkoutById(int id)
        {
            var workout = await _workoutService.GetWorkoutByIdAsync(id);
            if (workout == null) return NotFound();
            return Ok(workout);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkoutDto>>> GetAllWorkouts()
        {
            var workouts = await _workoutService.GetAllWorkoutsAsync();
            return Ok(workouts);
        }

        [HttpGet("type/{type}")]
        public async Task<ActionResult<IEnumerable<WorkoutDto>>> GetWorkoutsByType(WorkoutType type)
        {
            var workouts = await _workoutService.GetWorkoutsByTypeAsync(type);
            return Ok(workouts);
        }

        [HttpPost]
        public async Task<IActionResult> AddWorkout([FromBody] CreateWorkoutDto dto)
        {
            var authHeader = Request.Headers["Authorization"];
            Console.WriteLine($"Authorization Header: {authHeader}");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _workoutService.AddWorkoutAsync(dto);
            return CreatedAtAction(nameof(GetWorkoutById), new { id = dto.UserId }, dto);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateWorkout([FromBody] UpdateWorkoutDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _workoutService.UpdateWorkoutAsync(dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkout(int id)
        {
            await _workoutService.DeleteWorkoutAsync(id);
            return NoContent();
        }
    }
}
