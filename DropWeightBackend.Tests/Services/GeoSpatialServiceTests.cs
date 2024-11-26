using Moq;
using Xunit;
using DropWeightBackend.Api.Services;
using DropWeightBackend.Domain.Entities;
using DropWeightBackend.Infrastructure.UnitOfWork;
using DropWeightBackend.Infrastructure.Repositories.Interfaces;
using DropWeightBackend.Api.DTOs;
using DropWeightBackend.Api.Services.Implementations;

namespace DropWeightBackend.Tests
{
    public class GeoSpatialServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IGeoSpatialRepository> _mockGeoSpatialRepository;
        private readonly Mock<IWorkoutRepository> _mockWorkoutRepository;
        private readonly GeoSpatialService _geoSpatialService;
        private readonly Workout _testWorkout;
        private readonly User _testUser;

        public GeoSpatialServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockGeoSpatialRepository = new Mock<IGeoSpatialRepository>();
            _mockWorkoutRepository = new Mock<IWorkoutRepository>();
            _mockUnitOfWork.Setup(uow => uow.GeoSpatials).Returns(_mockGeoSpatialRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.Workouts).Returns(_mockWorkoutRepository.Object);
            _geoSpatialService = new GeoSpatialService(_mockUnitOfWork.Object);

            // Create test user first
            _testUser = new User
            {
                UserId = 1,
                Username = "testuser",
                FirstName = "Test",
                LastName = "User"
            };

            // Create test workout with required User property
            _testWorkout = new Workout
            {
                WorkoutId = 1,
                UserId = _testUser.UserId,
                User = _testUser,
            };
        }

        [Fact]
        public async Task GetGeoSpatialByIdAsync_ShouldReturnDto_WhenExists()
        {
            // Arrange
            var geoSpatial = new GeoSpatial
            {
                GeoSpatialId = 1,
                Latitude = 40,
                Longitude = 50,
                WorkoutId = 1,
                Workout = _testWorkout
            };

            _mockGeoSpatialRepository.Setup(repo => repo.GetGeoSpatialByIdAsync(1))
                .ReturnsAsync(geoSpatial);

            // Act
            var result = await _geoSpatialService.GetGeoSpatialByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(geoSpatial.GeoSpatialId, result.GeoSpatialId);
            Assert.Equal(geoSpatial.Latitude, result.Latitude);
            Assert.Equal(geoSpatial.Longitude, result.Longitude);
        }

        [Fact]
        public async Task GetGeoSpatialByIdAsync_ShouldReturnNull_WhenNotExists()
        {
            // Arrange
            _mockGeoSpatialRepository.Setup(repo => repo.GetGeoSpatialByIdAsync(999))
                .ReturnsAsync((GeoSpatial?)null);

            // Act
            var result = await _geoSpatialService.GetGeoSpatialByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllGeoSpatialsAsync_ShouldReturnAllDtos()
        {
            // Arrange
            var geoSpatials = new List<GeoSpatial>
            {
                new GeoSpatial { GeoSpatialId = 1, Latitude = 40, Longitude = 50, WorkoutId = 1, Workout = _testWorkout },
                new GeoSpatial { GeoSpatialId = 2, Latitude = 41, Longitude = 51, WorkoutId = 1, Workout = _testWorkout }
            };

            _mockGeoSpatialRepository.Setup(repo => repo.GetAllGeoSpatialsAsync())
                .ReturnsAsync(geoSpatials);

            // Act
            var result = await _geoSpatialService.GetAllGeoSpatialsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(geoSpatials.Count, result.Count());
            Assert.Collection(result,
                item => Assert.Equal(geoSpatials[0].GeoSpatialId, item.GeoSpatialId),
                item => Assert.Equal(geoSpatials[1].GeoSpatialId, item.GeoSpatialId)
            );
        }

        [Fact]
        public async Task GetGeoSpatialsByWorkoutIdAsync_ShouldReturnDtos()
        {
            // Arrange
            var geoSpatials = new List<GeoSpatial>
            {
                new GeoSpatial { GeoSpatialId = 1, Latitude = 40, Longitude = 50, WorkoutId = 1, Workout = _testWorkout },
                new GeoSpatial { GeoSpatialId = 2, Latitude = 41, Longitude = 51, WorkoutId = 1, Workout = _testWorkout }
            };

            _mockGeoSpatialRepository.Setup(repo => repo.GetGeoSpatialsByWorkoutIdAsync(1))
                .ReturnsAsync(geoSpatials);

            // Act
            var result = await _geoSpatialService.GetGeoSpatialsByWorkoutIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(geoSpatials.Count, result.Count());
        }

        [Fact]
        public async Task AddGeoSpatialAsync_ShouldAddAndSaveChanges()
        {
            // Arrange
            var createDto = new CreateGeoSpatialDto
            {
                Latitude = 40,
                Longitude = 50,
                WorkoutId = 1
            };

            _mockWorkoutRepository.Setup(repo => repo.GetWorkoutByIdAsync(1))
                .ReturnsAsync(_testWorkout);

            // Act
            await _geoSpatialService.AddGeoSpatialAsync(createDto);

            // Assert
            _mockGeoSpatialRepository.Verify(repo => 
                repo.AddGeoSpatialAsync(It.Is<GeoSpatial>(g => 
                    g.Latitude == createDto.Latitude && 
                    g.Longitude == createDto.Longitude && 
                    g.WorkoutId == createDto.WorkoutId)), 
                Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateGeoSpatialAsync_ShouldUpdateWhenExists()
        {
            // Arrange
            var updateDto = new UpdateGeoSpatialDto
            {
                GeoSpatialId = 1,
                Latitude = 40,
                Longitude = 50
            };

            var existingGeoSpatial = new GeoSpatial
            {
                GeoSpatialId = 1,
                Latitude = 30,
                Longitude = 40,
                WorkoutId = 1,
                Workout = _testWorkout
            };

            _mockGeoSpatialRepository.Setup(repo => repo.GetGeoSpatialByIdAsync(1))
                .ReturnsAsync(existingGeoSpatial);

            // Act
            await _geoSpatialService.UpdateGeoSpatialAsync(updateDto);

            // Assert
            _mockGeoSpatialRepository.Verify(repo => 
                repo.UpdateGeoSpatialAsync(It.Is<GeoSpatial>(g => 
                    g.GeoSpatialId == updateDto.GeoSpatialId && 
                    g.Latitude == updateDto.Latitude && 
                    g.Longitude == updateDto.Longitude)), 
                Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateGeoSpatialAsync_ShouldNotUpdateWhenNotExists()
        {
            // Arrange
            var updateDto = new UpdateGeoSpatialDto
            {
                GeoSpatialId = 999,
                Latitude = 40,
                Longitude = 50
            };

            _mockGeoSpatialRepository.Setup(repo => repo.GetGeoSpatialByIdAsync(999))
                .ReturnsAsync((GeoSpatial?)null);

            // Act
            await _geoSpatialService.UpdateGeoSpatialAsync(updateDto);

            // Assert
            _mockGeoSpatialRepository.Verify(repo => 
                repo.UpdateGeoSpatialAsync(It.IsAny<GeoSpatial>()), 
                Times.Never);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteGeoSpatialAsync_ShouldDeleteAndSaveChanges()
        {
            // Arrange
            int geoSpatialId = 1;

            // Act
            await _geoSpatialService.DeleteGeoSpatialAsync(geoSpatialId);

            // Assert
            _mockGeoSpatialRepository.Verify(repo => 
                repo.DeleteGeoSpatialAsync(geoSpatialId), 
                Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }
    }
} 