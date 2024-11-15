using System.Collections.Generic;
using System.Threading.Tasks;
using DropWeightBackend.Domain.Entities;
using DropWeightBackend.Api.DTOs;


namespace DropWeightBackend.Api.Services
{
    public interface IGeoSpatialService
    {
        Task<GeoSpatialDto> GetGeoSpatialByIdAsync(int geoSpatialId);
        Task<IEnumerable<GeoSpatialDto>> GetAllGeoSpatialsAsync();
        Task<IEnumerable<GeoSpatialDto>> GetGeoSpatialsByWorkoutIdAsync(int workoutId);
        Task AddGeoSpatialAsync(CreateGeoSpatialDto dto);
        Task UpdateGeoSpatialAsync(UpdateGeoSpatialDto dto);
        Task DeleteGeoSpatialAsync(int geoSpatialId);
    }
}