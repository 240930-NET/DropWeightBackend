using System;
using System.Threading.Tasks;
using DropWeightBackend.Infrastructure.Repositories.Interfaces;

namespace DropWeightBackend.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        // Repositories
        IWorkoutRepository Workouts { get; }
        IGeoSpatialRepository GeoSpatials { get; }
        IUserRepository Users { get; }
        INutritionRepository Nutritions { get; }
        IWorkoutScheduleRepository WorkoutSchedules { get; }
        IGoalRepository Goals { get; }

        // Commit changes
        Task<int> CompleteAsync();
    }

}
