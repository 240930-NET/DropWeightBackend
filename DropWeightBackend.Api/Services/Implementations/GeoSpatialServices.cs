using System.Collections.Generic;
using System.Threading.Tasks;
using DropWeightBackend.Domain.Entities;
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

        public async Task<GeoSpatial> GetGeoSpatialByIdAsync(int geoSpatialId)
        {
            return await _geoSpatialRepository.GetGeoSpatialByIdAsync(geoSpatialId);
        }

        public async Task<IEnumerable<GeoSpatial>> GetAllGeoSpatialsAsync()
        {
            return await _geoSpatialRepository.GetAllGeoSpatialsAsync();
        }

        public async Task<IEnumerable<GeoSpatial>> GetGeoSpatialsByWorkoutIdAsync(int workoutId)
        {
            return await _geoSpatialRepository.GetGeoSpatialsByWorkoutIdAsync(workoutId);
        }

        public async Task AddGeoSpatialAsync(GeoSpatial geoSpatial)
        {
            await _geoSpatialRepository.AddGeoSpatialAsync(geoSpatial);
        }

        public async Task UpdateGeoSpatialAsync(GeoSpatial geoSpatial)
        {
            await _geoSpatialRepository.UpdateGeoSpatialAsync(geoSpatial);
        }

        public async Task DeleteGeoSpatialAsync(int geoSpatialId)
        {
            await _geoSpatialRepository.DeleteGeoSpatialAsync(geoSpatialId);
        }
    }
}
