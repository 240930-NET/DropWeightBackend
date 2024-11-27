using DropWeightBackend.Api.Services.Interfaces;
using DropWeightBackend.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DropWeightBackend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NutritionController : ControllerBase
    {
        private readonly INutritionService _nutritionService;

        public NutritionController(INutritionService nutritionService)
        {
            _nutritionService = nutritionService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Nutrition>>> GetAllNutrition()
        {
            var nutritions = await _nutritionService.GetAllNutritionsAsync();
            return Ok(nutritions);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Nutrition>> GetNutritionById(int id)
        {
            var nutrition = await _nutritionService.GetNutritionByIdAsync(id);
            if (nutrition == null)
            {
                return NotFound();
            }
            return Ok(nutrition);
        }

        [Authorize]
        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<Nutrition>>> GetAllNutrition(int id)
        {
            var nutritions = await _nutritionService.GetNutritionsByUserIdAsync(id);
            return Ok(nutritions);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Nutrition>> CreateNutrition([FromBody] Nutrition nutrition)
        {
            await _nutritionService.AddNutritionAsync(nutrition);
            return Ok();
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateNutrition([FromBody] Nutrition nutrition)
        {
            await _nutritionService.UpdateNutritionAsync(nutrition);
            return Ok();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNutrition(int id)
        {
            var nutrition = await _nutritionService.GetNutritionByIdAsync(id);
            if (nutrition == null)
            {
                return NotFound("Nutrition not found.");
            }
            await _nutritionService.DeleteNutritionAsync(id);
            return Ok();
        }
    }
}