using Xunit;
using Microsoft.EntityFrameworkCore;
using DropWeightBackend.Infrastructure.Data;
using DropWeightBackend.Infrastructure.Repositories.Implementations;
using DropWeightBackend.Domain.Entities;
using DropWeightBackend.Domain.Enums;

namespace DropWeightBackend.Tests.Repositories
{
    public class WorkoutRepositoryTests : IDisposable
    {
        private readonly DbContextOptions<DropWeightContext> _options;
        private readonly DropWeightContext _context;
        private readonly WorkoutRepository _repository;
        private readonly User _testUser;

        public WorkoutRepositoryTests()
        {
            var dbName = Guid.NewGuid().ToString();
            _options = new DbContextOptionsBuilder<DropWeightContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            _context = new DropWeightContext(_options);
            _repository = new WorkoutRepository(_context);

            // Create test user
            _testUser = new User
            {
                UserId = 1,
                Username = "testuser",
                FirstName = "Test",
                LastName = "User",
                PasswordHash = "hash",
                PasswordSalt = "salt"
            };
            _context.Users.Add(_testUser);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task GetWorkoutByIdAsync_ShouldReturnWorkout_WhenExists()
        {
            // Arrange
            var workout = new Workout
            {
                WorkoutId = 1,
                Type = WorkoutType.Run,
                UserId = _testUser.UserId,
                User = _testUser
            };
            await _context.Workouts.AddAsync(workout);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetWorkoutByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(WorkoutType.Run, result.Type);
        }

        [Fact]
        public async Task GetAllWorkoutsAsync_ShouldReturnAllWorkouts()
        {
            // Arrange
            var workouts = new List<Workout>
            {
                new Workout { WorkoutId = 2, Type = WorkoutType.Run, UserId = _testUser.UserId },
                new Workout { WorkoutId = 3, Type = WorkoutType.Lift, UserId = _testUser.UserId }
            };
            await _context.Workouts.AddRangeAsync(workouts);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllWorkoutsAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetWorkoutsByTypeAsync_ShouldReturnWorkoutsOfType()
        {
            // Arrange
            var workouts = new List<Workout>
            {
                new Workout { WorkoutId = 4, Type = WorkoutType.Run, UserId = _testUser.UserId },
                new Workout { WorkoutId = 5, Type = WorkoutType.Lift, UserId = _testUser.UserId }
            };
            await _context.Workouts.AddRangeAsync(workouts);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetWorkoutsByTypeAsync(WorkoutType.Run);

            // Assert
            Assert.Single(result);
            Assert.All(result, w => Assert.Equal(WorkoutType.Run, w.Type));
        }

        [Fact]
        public async Task AddWorkoutAsync_ShouldAddWorkout()
        {
            // Arrange
            var workout = new Workout
            {
                WorkoutId = 6,
                Type = WorkoutType.Run,
                UserId = _testUser.UserId
            };

            // Act
            await _repository.AddWorkoutAsync(workout);

            // Assert
            var addedWorkout = await _context.Workouts.FindAsync(6);
            Assert.NotNull(addedWorkout);
            Assert.Equal(WorkoutType.Run, addedWorkout.Type);
        }

        [Fact]
        public async Task UpdateWorkoutAsync_ShouldUpdateWorkout()
        {
            // Arrange
            var workout = new Workout
            {
                WorkoutId = 7,
                Type = WorkoutType.Run,
                UserId = _testUser.UserId
            };
            await _context.Workouts.AddAsync(workout);
            await _context.SaveChangesAsync();

            // Act
            workout.Type = WorkoutType.Lift;
            await _repository.UpdateWorkoutAsync(workout);

            // Assert
            var updatedWorkout = await _context.Workouts.FindAsync(7);
            Assert.NotNull(updatedWorkout);
            Assert.Equal(WorkoutType.Lift, updatedWorkout.Type);
        }

        [Fact]
        public async Task DeleteWorkoutAsync_ShouldDeleteWorkout()
        {
            // Arrange
            var workout = new Workout
            {
                WorkoutId = 8,
                Type = WorkoutType.Run,
                UserId = _testUser.UserId
            };
            await _context.Workouts.AddAsync(workout);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteWorkoutAsync(8);

            // Assert
            var deletedWorkout = await _context.Workouts.FindAsync(8);
            Assert.Null(deletedWorkout);
        }

        [Fact]
        public async Task DeleteWorkoutAsync_ShouldNotThrow_WhenWorkoutDoesNotExist()
        {
            // Act & Assert
            await _repository.DeleteWorkoutAsync(999); // Should not throw
        }
    }
} 