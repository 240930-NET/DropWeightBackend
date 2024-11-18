using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DropWeightBackend.Api.DTOs;
using DropWeightBackend.Domain.Entities;
using DropWeightBackend.Api.Services.Interfaces;
using DropWeightBackend.Infrastructure.UnitOfWork;

namespace DropWeightBackend.Api.Services
{
    public class GeoSpatialService : IGeoSpatialService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GeoSpatialService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<GeoSpatialDto> GetGeoSpatialByIdAsync(int geoSpatialId)
        {
            var geoSpatial = await _unitOfWork.GeoSpatials.GetGeoSpatialByIdAsync(geoSpatialId);
            return geoSpatial == null ? null : MapToDto(geoSpatial);
        }

        public async Task<IEnumerable<GeoSpatialDto>> GetAllGeoSpatialsAsync()
        {
            var geoSpatials = await _unitOfWork.GeoSpatials.GetAllGeoSpatialsAsync();
            return geoSpatials.Select(MapToDto);
        }

        public async Task<IEnumerable<GeoSpatialDto>> GetGeoSpatialsByWorkoutIdAsync(int workoutId)
        {
            var geoSpatials = await _unitOfWork.GeoSpatials.GetGeoSpatialsByWorkoutIdAsync(workoutId);
            return geoSpatials.Select(MapToDto);
        }

        public async Task AddGeoSpatialAsync(CreateGeoSpatialDto dto)
        {
            var geoSpatial = new GeoSpatial
            {
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                WorkoutId = dto.WorkoutId,
                Workout = await _unitOfWork.Workouts.GetWorkoutByIdAsync(dto.WorkoutId)
            };

            await _unitOfWork.GeoSpatials.AddGeoSpatialAsync(geoSpatial);
            await _unitOfWork.CompleteAsync(); // Save changes
        }

        public async Task UpdateGeoSpatialAsync(UpdateGeoSpatialDto dto)
        {
            var geoSpatial = await _unitOfWork.GeoSpatials.GetGeoSpatialByIdAsync(dto.GeoSpatialId);
            if (geoSpatial == null) return;

            geoSpatial.Latitude = dto.Latitude;
            geoSpatial.Longitude = dto.Longitude;

            await _unitOfWork.GeoSpatials.UpdateGeoSpatialAsync(geoSpatial);
            await _unitOfWork.CompleteAsync(); // Save changes
        }

        public async Task DeleteGeoSpatialAsync(int geoSpatialId)
        {
            await _unitOfWork.GeoSpatials.DeleteGeoSpatialAsync(geoSpatialId);
            await _unitOfWork.CompleteAsync(); // Save changes
        }

        private GeoSpatialDto MapToDto(GeoSpatial geoSpatial)
        {
            return new GeoSpatialDto
            {
                GeoSpatialId = geoSpatial.GeoSpatialId,
                Latitude = geoSpatial.Latitude,
                Longitude = geoSpatial.Longitude
            };
        }
    }
}
