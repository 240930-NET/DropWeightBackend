using DropWeight.Infrastructure.Data;
using DropWeight.Domain.Entities;
using DropWeight.Domain.Repositories;
using DropWeight.Infrastructure.Repositories;

namespace DropWeight.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly DropWeightContext _context;

        // Repositories
        //public IStudentRepository Students { get; }
        /*public IClassRepository Classes { get; }
        public IAssignmentRepository Assignments { get; }
        public IGradeRepository Grades { get; }
        public IUserRepository Users { get; }
        public IInstitutionRepository Institutions { get; }*/

        public IWorkoutRepository Workouts { get; }
        public IGeoSpatialRepository GeoSpatials { get; }

        public UnitOfWork(DropWeightContext context)
        {
            _context = context;

            // Initialize repositories
            //Students = new StudentRepository(_context);
            /*Classes = new ClassRepository(_context);
            Assignments = new AssignmentRepository(_context);
            Grades = new GradeRepository(_context);
            Users = new UserRepository(_context);
            Institutions = new InstitutionRepository(_context);*/

            Workouts = new WorkoutRepository(_context);
            GeoSpatials = new GeoSpatialRepository(_context);
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
