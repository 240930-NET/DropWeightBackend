using System;
using System.Threading.Tasks;
using DropWeight.Domain.Repositories;

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
