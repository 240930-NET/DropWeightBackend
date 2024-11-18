using DropWeightBackend.Infrastructure.Data;
using DropWeightBackend.Domain.Entities;
using DropWeightBackend.Infrastructure.Repositories.Interfaces;
using DropWeightBackend.Infrastructure.Repositories.Implementations;

namespace DropWeightBackend.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly DropWeightContext _context;

        // Repositories
        public IWorkoutRepository Workouts { get; }
        public IGeoSpatialRepository GeoSpatials { get; }
        public IUserRepository Users { get; }
        public INutritionRepository Nutritions { get; }
        public IWorkoutScheduleRepository WorkoutSchedules { get; }
        public IGoalRepository Goals { get; }

        public UnitOfWork(DropWeightContext context)
        {
            _context = context;

            // Initialize repositories

            Workouts = new WorkoutRepository(_context);
            GeoSpatials = new GeoSpatialRepository(_context);
            Users = new UserRepository(_context);
            Nutritions = new NutritionRepository(_context);
            WorkoutSchedules = new WorkoutScheduleRepository(_context);
            Goals = new GoalRepository(context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        #region IDisposable Support
        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose the context
                    _context.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
