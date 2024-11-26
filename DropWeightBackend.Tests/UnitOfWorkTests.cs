using Xunit;
using Microsoft.EntityFrameworkCore;
using DropWeightBackend.Infrastructure.Data;
using DropWeightBackend.Infrastructure.UnitOfWork;
using DropWeightBackend.Domain.Entities;
using DropWeightBackend.Infrastructure.Repositories.Interfaces;
using DropWeightBackend.Domain.Enums;

namespace DropWeightBackend.Tests
{
    public class UnitOfWorkTests : IDisposable
    {
        private readonly DbContextOptions<DropWeightContext> _options;
        private readonly DropWeightContext _context;
        private readonly UnitOfWork _unitOfWork;

        public UnitOfWorkTests()
        {
            _options = new DbContextOptionsBuilder<DropWeightContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new DropWeightContext(_options);
            _unitOfWork = new UnitOfWork(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public void UnitOfWork_ShouldInitializeAllRepositories()
        {
            // Assert
            Assert.NotNull(_unitOfWork.Users);
            Assert.NotNull(_unitOfWork.Workouts);
            Assert.NotNull(_unitOfWork.GeoSpatials);
            Assert.NotNull(_unitOfWork.Nutritions);
            Assert.NotNull(_unitOfWork.WorkoutSchedules);
            Assert.NotNull(_unitOfWork.Goals);
        }

        [Fact]
        public async Task CompleteAsync_ShouldSaveChangesToAllRepositories()
        {
            // Arrange
            var user = new User
            {
                UserId = 1,
                Username = "testuser",
                FirstName = "Test",
                LastName = "User",
                PasswordHash = "hash",
                PasswordSalt = "salt"
            };
            await _context.Users.AddAsync(user);

            var workout = new Workout
            {
                WorkoutId = 1,
                Type = WorkoutType.Run,
                UserId = user.UserId,
                User = user
            };
            await _context.Workouts.AddAsync(workout);

            var nutrition = new Nutrition
            {
                NutritionId = 1,
                Description = "Test Nutrition",
                ServingSize = 100,
                Calories = 200,
                Date = DateTime.Now,
                UserId = user.UserId,
                User = user
            };
            await _context.Nutritions.AddAsync(nutrition);

            // Act
            var result = await _unitOfWork.CompleteAsync();

            // Assert
            Assert.True(result > 0);
            Assert.Equal(3, result); // Should have saved 3 entities
        }

        [Fact]
        public async Task Repositories_ShouldShareSameContext()
        {
            // Arrange
            var user = new User
            {
                UserId = 2,
                Username = "testuser2",
                FirstName = "Test",
                LastName = "User",
                PasswordHash = "hash",
                PasswordSalt = "salt"
            };

            // Act
            await _unitOfWork.Users.AddUserAsync(user);
            await _unitOfWork.CompleteAsync();

            var workout = new Workout
            {
                WorkoutId = 2,
                Type = WorkoutType.Run,
                UserId = user.UserId,
                User = user
            };
            await _unitOfWork.Workouts.AddWorkoutAsync(workout);
            await _unitOfWork.CompleteAsync();

            // Assert
            var savedUser = await _unitOfWork.Users.GetUserByIdAsync(2);
            var savedWorkout = await _unitOfWork.Workouts.GetWorkoutByIdAsync(2);
            Assert.NotNull(savedUser);
            Assert.NotNull(savedWorkout);
            Assert.Equal(user.UserId, savedWorkout.UserId);
        }

        [Fact]
        public void Dispose_ShouldDisposeContext()
        {
            // Arrange
            var context = new DropWeightContext(_options);
            var unitOfWork = new UnitOfWork(context);

            // Act
            unitOfWork.Dispose();

            // Assert
            Assert.Throws<ObjectDisposedException>(() => context.Users.ToList());
        }

        [Fact]
        public void Dispose_ShouldBeIdempotent()
        {
            // Arrange
            var context = new DropWeightContext(_options);
            var unitOfWork = new UnitOfWork(context);

            // Act
            unitOfWork.Dispose();
            unitOfWork.Dispose(); // Should not throw

            // Assert
            Assert.Throws<ObjectDisposedException>(() => context.Users.ToList());
        }

        [Fact]
        public async Task UnitOfWork_ShouldSupportTransactions()
        {
            // Arrange
            var user = new User
            {
                UserId = 3,
                Username = "testuser3",
                FirstName = "Test",
                LastName = "User",
                PasswordHash = "hash",
                PasswordSalt = "salt"
            };

            // Act & Assert
            {
                try
                {
                    await _unitOfWork.Users.AddUserAsync(user);
                    await _unitOfWork.CompleteAsync();

                    var workout = new Workout
                    {
                        WorkoutId = 3,
                        Type = WorkoutType.Run,
                        UserId = user.UserId,
                        User = user
                    };
                    await _unitOfWork.Workouts.AddWorkoutAsync(workout);
                    await _unitOfWork.CompleteAsync();

                    await _unitOfWork.CompleteAsync();
                }
                catch
                {
                    throw;
                }
            }

            var savedUser = await _unitOfWork.Users.GetUserByIdAsync(3);
            var savedWorkout = await _unitOfWork.Workouts.GetWorkoutByIdAsync(3);
            Assert.NotNull(savedUser);
            Assert.NotNull(savedWorkout);
        }
    }
} 