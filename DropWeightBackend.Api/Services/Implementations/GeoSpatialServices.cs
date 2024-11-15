using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DropWeightBackend.Api.DTOs;
using DropWeightBackend.Domain.Entities;
using DropWeightBackend.Infrastructure.Repositories.Implementations;
using DropWeightBackend.Api.Services;
using DropWeightBackend.Infrastructure.Repositories.Interfaces;

namespace DropWeightBackend.Api.Services
{
    public class GeoSpatialService : IGeoSpatialService
    {
        private readonly IGeoSpatialRepository _geoSpatialRepository;

        public GeoSpatialService(IGeoSpatialRepository geoSpatialRepository)
        {
            _geoSpatialRepository = geoSpatialRepository;
        }

        public async Task<GeoSpatialDto> GetGeoSpatialByIdAsync(int geoSpatialId)
        {
            var geoSpatial = await _geoSpatialRepository.GetGeoSpatialByIdAsync(geoSpatialId);
            return geoSpatial == null ? null : MapToDto(geoSpatial);
        }

        public async Task<IEnumerable<GeoSpatialDto>> GetAllGeoSpatialsAsync()
        {
            var geoSpatials = await _geoSpatialRepository.GetAllGeoSpatialsAsync();
            return geoSpatials.Select(MapToDto);
        }

        public async Task<IEnumerable<GeoSpatialDto>> GetGeoSpatialsByWorkoutIdAsync(int workoutId)
        {
            var geoSpatials = await _geoSpatialRepository.GetGeoSpatialsByWorkoutIdAsync(workoutId);
            return geoSpatials.Select(MapToDto);
        }

        public async Task AddGeoSpatialAsync(CreateGeoSpatialDto dto)
        {
            var geoSpatial = new GeoSpatial
            {
                Latitude = dto.Latitude,
                Longitude = dto.Longitude
            };
            await _geoSpatialRepository.AddGeoSpatialAsync(geoSpatial);
        }

        public async Task UpdateGeoSpatialAsync(UpdateGeoSpatialDto dto)
        {
            var geoSpatial = await _geoSpatialRepository.GetGeoSpatialByIdAsync(dto.GeoSpatialId);
            if (geoSpatial == null) return;

            geoSpatial.Latitude = dto.Latitude;
            geoSpatial.Longitude = dto.Longitude;
            await _geoSpatialRepository.UpdateGeoSpatialAsync(geoSpatial);
        }

        public async Task DeleteGeoSpatialAsync(int geoSpatialId)
        {
            await _geoSpatialRepository.DeleteGeoSpatialAsync(geoSpatialId);
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
