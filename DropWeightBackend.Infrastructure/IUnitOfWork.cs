using System;
using System.Threading.Tasks;
using DropWeight.Domain.Repositories;

namespace DropWeight.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        // Repositories
        IWorkoutRepository Classes { get; }
        IGeoSpatialRepository Assignments { get; }

        // Commit changes
        Task<int> CompleteAsync();
    }

}
