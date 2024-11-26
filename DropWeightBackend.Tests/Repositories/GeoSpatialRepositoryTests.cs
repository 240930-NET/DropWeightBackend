using Xunit;
using Microsoft.EntityFrameworkCore;
using DropWeightBackend.Infrastructure.Data;
using DropWeightBackend.Infrastructure.Repositories.Implementations;
using DropWeightBackend.Domain.Entities;

namespace DropWeightBackend.Tests.Repositories
{
    public class GeoSpatialRepositoryTests : IDisposable
    {
        private readonly DbContextOptions<DropWeightContext> _options;
        private readonly DropWeightContext _context;
        private readonly GeoSpatialRepository _repository;
        private readonly User _testUser;
        private readonly Workout _testWorkout;

        public GeoSpatialRepositoryTests()
        {
            var dbName = Guid.NewGuid().ToString();
            _options = new DbContextOptionsBuilder<DropWeightContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            _context = new DropWeightContext(_options);
            _repository = new GeoSpatialRepository(_context);

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

            // Create test workout
            _testWorkout = new Workout
            {
                WorkoutId = 1,
                UserId = _testUser.UserId,
                User = _testUser
            };
            _context.Workouts.Add(_testWorkout);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task GetGeoSpatialByIdAsync_ShouldReturnGeoSpatial_WhenExists()
        {
            // Arrange
            var geoSpatial = new GeoSpatial
            {
                GeoSpatialId = 1,
                Latitude = 40,
                Longitude = 50,
                WorkoutId = _testWorkout.WorkoutId
            };
            await _context.GeoSpatials.AddAsync(geoSpatial);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetGeoSpatialByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(40, result.Latitude);
            Assert.Equal(50, result.Longitude);
        }

        [Fact]
        public async Task GetAllGeoSpatialsAsync_ShouldReturnAllGeoSpatials()
        {
            // Arrange
            var geoSpatials = new List<GeoSpatial>
            {
                new GeoSpatial { GeoSpatialId = 2, Latitude = 40, Longitude = 50, WorkoutId = _testWorkout.WorkoutId },
                new GeoSpatial { GeoSpatialId = 3, Latitude = 41, Longitude = 51, WorkoutId = _testWorkout.WorkoutId }
            };
            await _context.GeoSpatials.AddRangeAsync(geoSpatials);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllGeoSpatialsAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetGeoSpatialsByWorkoutIdAsync_ShouldReturnWorkoutGeoSpatials()
        {
            // Arrange
            var geoSpatials = new List<GeoSpatial>
            {
                new GeoSpatial { GeoSpatialId = 4, Latitude = 40, Longitude = 50, WorkoutId = _testWorkout.WorkoutId },
                new GeoSpatial { GeoSpatialId = 5, Latitude = 41, Longitude = 51, WorkoutId = _testWorkout.WorkoutId }
            };
            await _context.GeoSpatials.AddRangeAsync(geoSpatials);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetGeoSpatialsByWorkoutIdAsync(_testWorkout.WorkoutId);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.All(result, g => Assert.Equal(_testWorkout.WorkoutId, g.WorkoutId));
        }

        [Fact]
        public async Task AddGeoSpatialAsync_ShouldAddGeoSpatial()
        {
            // Arrange
            var geoSpatial = new GeoSpatial
            {
                GeoSpatialId = 6,
                Latitude = 40,
                Longitude = 50,
                WorkoutId = _testWorkout.WorkoutId
            };

            // Act
            await _repository.AddGeoSpatialAsync(geoSpatial);

            // Assert
            var addedGeoSpatial = await _context.GeoSpatials.FindAsync(6);
            Assert.NotNull(addedGeoSpatial);
            Assert.Equal(40, addedGeoSpatial.Latitude);
            Assert.Equal(50, addedGeoSpatial.Longitude);
        }

        [Fact]
        public async Task UpdateGeoSpatialAsync_ShouldUpdateGeoSpatial()
        {
            // Arrange
            var geoSpatial = new GeoSpatial
            {
                GeoSpatialId = 7,
                Latitude = 40,
                Longitude = 50,
                WorkoutId = _testWorkout.WorkoutId
            };
            await _context.GeoSpatials.AddAsync(geoSpatial);
            await _context.SaveChangesAsync();

            // Act
            geoSpatial.Latitude = 42;
            geoSpatial.Longitude = 52;
            await _repository.UpdateGeoSpatialAsync(geoSpatial);

            // Assert
            var updatedGeoSpatial = await _context.GeoSpatials.FindAsync(7);
            Assert.NotNull(updatedGeoSpatial);
            Assert.Equal(42, updatedGeoSpatial.Latitude);
            Assert.Equal(52, updatedGeoSpatial.Longitude);
        }

        [Fact]
        public async Task DeleteGeoSpatialAsync_ShouldDeleteGeoSpatial()
        {
            // Arrange
            var geoSpatial = new GeoSpatial
            {
                GeoSpatialId = 8,
                Latitude = 40,
                Longitude = 50,
                WorkoutId = _testWorkout.WorkoutId
            };
            await _context.GeoSpatials.AddAsync(geoSpatial);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteGeoSpatialAsync(8);

            // Assert
            var deletedGeoSpatial = await _context.GeoSpatials.FindAsync(8);
            Assert.Null(deletedGeoSpatial);
        }

        [Fact]
        public async Task DeleteGeoSpatialAsync_ShouldNotThrow_WhenGeoSpatialDoesNotExist()
        {
            // Act & Assert
            await _repository.DeleteGeoSpatialAsync(999); // Should not throw
        }
    }
} 