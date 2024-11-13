using System.Collections.Generic;
using System.Threading.Tasks;
using DropWeight.Domain.Entities;

namespace DropWeight.Domain.Repositories
{
    public interface IGeoSpatialRepository
    {
        Task<GeoSpatial> GetGeoSpatialByIdAsync(int geoSpatialId);
        Task<IEnumerable<GeoSpatial>> GetAllGeoSpatialsAsync();
        Task<IEnumerable<GeoSpatial>> GetGeoSpatialsByWorkoutIdAsync(int workoutId);
        Task AddGeoSpatialAsync(GeoSpatial geoSpatial);
        Task UpdateGeoSpatialAsync(GeoSpatial geoSpatial);
        Task DeleteGeoSpatialAsync(int geoSpatialId);
    }
}
