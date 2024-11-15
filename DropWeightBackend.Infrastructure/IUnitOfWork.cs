using System;
using System.Threading.Tasks;
using DropWeightBackend.Infrastructure.Repositories.Interfaces;

namespace DropWeight.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        // Repositories
        IWorkoutRepository Workouts { get; }
        IGeoSpatialRepository GeoSpatials { get; }

        // Commit changes
        Task<int> CompleteAsync();
    }

}
