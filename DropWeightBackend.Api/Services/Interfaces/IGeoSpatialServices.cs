using System.Collections.Generic;
using System.Threading.Tasks;
using DropWeightBackend.Domain.Entities;

namespace DropWeightBackend.Api.Services
{
    public interface IGeoSpatialService
    {
        Task<GeoSpatial> GetGeoSpatialByIdAsync(int geoSpatialId);
        Task<IEnumerable<GeoSpatial>> GetAllGeoSpatialsAsync();
        Task<IEnumerable<GeoSpatial>> GetGeoSpatialsByWorkoutIdAsync(int workoutId);
        Task AddGeoSpatialAsync(GeoSpatial geoSpatial);
        Task UpdateGeoSpatialAsync(GeoSpatial geoSpatial);
        Task DeleteGeoSpatialAsync(int geoSpatialId);
    }
}
