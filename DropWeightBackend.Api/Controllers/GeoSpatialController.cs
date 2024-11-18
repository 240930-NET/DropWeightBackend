using System.Collections.Generic;
using System.Threading.Tasks;
using DropWeightBackend.Api.DTOs;
using DropWeightBackend.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DropWeightBackend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GeoSpatialController : ControllerBase
    {
        private readonly IGeoSpatialService _geoSpatialService;

        public GeoSpatialController(IGeoSpatialService geoSpatialService)
        {
            _geoSpatialService = geoSpatialService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GeoSpatialDto>> GetGeoSpatialById(int id)
        {
            var geoSpatial = await _geoSpatialService.GetGeoSpatialByIdAsync(id);
            if (geoSpatial == null) return NotFound();
            return Ok(geoSpatial);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GeoSpatialDto>>> GetAllGeoSpatials()
        {
            var geoSpatials = await _geoSpatialService.GetAllGeoSpatialsAsync();
            return Ok(geoSpatials);
        }

        [HttpGet("workout/{workoutId}")]
        public async Task<ActionResult<IEnumerable<GeoSpatialDto>>> GetGeoSpatialsByWorkoutId(int workoutId)
        {
            var geoSpatials = await _geoSpatialService.GetGeoSpatialsByWorkoutIdAsync(workoutId);
            return Ok(geoSpatials);
        }

        [HttpPost]
        public async Task<IActionResult> AddGeoSpatial([FromBody] CreateGeoSpatialDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _geoSpatialService.AddGeoSpatialAsync(dto);
            return CreatedAtAction(nameof(GetGeoSpatialById), new { id = dto.WorkoutId }, dto);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateGeoSpatial([FromBody] UpdateGeoSpatialDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _geoSpatialService.UpdateGeoSpatialAsync(dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGeoSpatial(int id)
        {
            await _geoSpatialService.DeleteGeoSpatialAsync(id);
            return NoContent();
        }
    }
}
