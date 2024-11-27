using DropWeightBackend.Api.DTOs;
using DropWeightBackend.Api.Services.Interfaces;
using DropWeightBackend.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DropWeightBackend.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class GoalController : Controller {

    public readonly IGoalService _goalService;

    public GoalController(IGoalService goalService) {
        _goalService = goalService;
    }


    //[Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllGoals() {

        try {
            return Ok(await _goalService.GetAllGoals());
        }
        catch(Exception e) {
            return BadRequest(e.Message);
        }
    }

    
    //[Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetGoalById(int id) {
        try {
            var goal = await _goalService.GetGoalById(id);
            if (goal == null)
            {
                return NotFound($"Goal with id: {id} not found.");
            }
            return Ok(await _goalService.GetGoalById(id));
        }
        catch(Exception e) {
            return BadRequest(e.Message);
        }
    }


    //[Authorize]
    [HttpPost]
    public async Task<IActionResult> AddGoal([FromBody] GoalDto goalDTO) {

        try {
            await _goalService.AddGoal(goalDTO);
            return Ok(goalDTO);
        }
        catch(Exception e) {
            Console.WriteLine("catch:" + goalDTO);
            return BadRequest(e.Message);
        }
    }


    //[Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGoal([FromBody] GoalDto goalDTO, int id) {
        try {
            await _goalService.UpdateGoal(goalDTO, id);
            return Ok(goalDTO);
        }
        catch(Exception e) {
            return BadRequest(e.Message);
        }
    }


    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGoal(int id) {
        try {
            await _goalService.DeleteGoal(id);
            return Ok();
        }
        catch(Exception e) {
            return BadRequest(e.Message);
        }
    }

}