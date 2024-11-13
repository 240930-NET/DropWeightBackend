using Microsoft.EntityFrameworkCore;
using DropWeightBackend.Domain.Entities;
using DropWeightBackend.Infrastructure.Data;
using DropWeightBackend.Infrastructure.Repositories.Interfaces;

namespace DropWeightBackend.Infrastructure.Repositories.Implementations
{
    public class GeoSpatialRepository : IGeoSpatialRepository
    {
        private readonly DropWeightContext _context;

        public GeoSpatialRepository(DropWeightContext context)
        {
            _context = context;
        }

        public async Task<GeoSpatial?> GetGeoSpatialByIdAsync(int geoSpatialId)
        {
            return await _context.GeoSpatials
                .FirstOrDefaultAsync(g => g.GeoSpatialId == geoSpatialId);
        }

        public async Task<IEnumerable<GeoSpatial>> GetAllGeoSpatialsAsync()
        {
            return await _context.GeoSpatials.ToListAsync();
        }

        public async Task<IEnumerable<GeoSpatial>> GetGeoSpatialsByWorkoutIdAsync(int workoutId)
        {
            return await _context.GeoSpatials
                .Where(g => g.WorkoutId == workoutId)
                .ToListAsync();
        }

        public async Task AddGeoSpatialAsync(GeoSpatial geoSpatial)
        {
            await _context.GeoSpatials.AddAsync(geoSpatial);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateGeoSpatialAsync(GeoSpatial geoSpatial)
        {
            _context.GeoSpatials.Update(geoSpatial);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteGeoSpatialAsync(int geoSpatialId)
        {
            var geoSpatial = await GetGeoSpatialByIdAsync(geoSpatialId);
            if (geoSpatial != null)
            {
                _context.GeoSpatials.Remove(geoSpatial);
                await _context.SaveChangesAsync();
            }
        }
    }
}
