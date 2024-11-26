using Xunit;
using Moq;
using DropWeightBackend.Api.Services.Implementations;
using DropWeightBackend.Domain.Entities;
using DropWeightBackend.Infrastructure.UnitOfWork;
using DropWeightBackend.Infrastructure.Repositories.Interfaces;
using DropWeightBackend.Domain.Enums;
using DropWeightBackend.Api.DTOs;

namespace DropWeightBackend.Tests
{
    public class WorkoutServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IWorkoutRepository> _mockWorkoutRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly WorkoutService _workoutService;
        private readonly User _testUser;
        private readonly WorkoutType _testWorkoutType = (WorkoutType)0;

        public WorkoutServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockWorkoutRepository = new Mock<IWorkoutRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockUnitOfWork.Setup(uow => uow.Workouts).Returns(_mockWorkoutRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.Users).Returns(_mockUserRepository.Object);
            _workoutService = new WorkoutService(_mockUnitOfWork.Object);

            _testUser = new User
            {
                UserId = 1,
                Username = "testuser",
                FirstName = "Test",
                LastName = "User"
            };
        }

        [Fact]
        public async Task GetWorkoutByIdAsync_ShouldReturnDto_WhenWorkoutExists()
        {
            // Arrange
            var workout = new Workout
            {
                WorkoutId = 1,
                StartTime = 1000,
                EndTime = 2000,
                Type = _testWorkoutType,
                Reps = 10,
                UserId = _testUser.UserId,
                User = _testUser,
                GeoSpatials = new List<GeoSpatial>
                {
                    new GeoSpatial { GeoSpatialId = 1, Latitude = 40, Longitude = 50 }
                }
            };

            _mockWorkoutRepository.Setup(repo => repo.GetWorkoutByIdAsync(1))
                .ReturnsAsync(workout);

            // Act
            var result = await _workoutService.GetWorkoutByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(workout.WorkoutId, result.WorkoutId);
            Assert.Equal(workout.Type, result.Type);
            Assert.Equal(workout.Reps, result.Reps);
            Assert.NotNull(result.GeoSpatials);
            Assert.Single(result.GeoSpatials);
        }

        [Fact]
        public async Task GetWorkoutByIdAsync_ShouldReturnNull_WhenWorkoutDoesNotExist()
        {
            // Arrange
            _mockWorkoutRepository.Setup(repo => repo.GetWorkoutByIdAsync(999))
                .ReturnsAsync((Workout?)null);

            // Act
            var result = await _workoutService.GetWorkoutByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllWorkoutsAsync_ShouldReturnAllWorkouts()
        {
            // Arrange
            var workouts = new List<Workout>
            {
                new Workout
                {
                    WorkoutId = 1,
                    Type = _testWorkoutType,
                    UserId = _testUser.UserId,
                    User = _testUser
                },
                new Workout
                {
                    WorkoutId = 2,
                    Type = _testWorkoutType,
                    UserId = _testUser.UserId,
                    User = _testUser
                }
            };

            _mockWorkoutRepository.Setup(repo => repo.GetAllWorkoutsAsync())
                .ReturnsAsync(workouts);

            // Act
            var result = await _workoutService.GetAllWorkoutsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetWorkoutsByTypeAsync_ShouldReturnWorkoutsOfSpecificType()
        {
            // Arrange
            var workouts = new List<Workout>
            {
                new Workout
                {
                    WorkoutId = 1,
                    Type = _testWorkoutType,
                    UserId = _testUser.UserId,
                    User = _testUser
                }
            };

            _mockWorkoutRepository.Setup(repo => repo.GetWorkoutsByTypeAsync(_testWorkoutType))
                .ReturnsAsync(workouts);

            // Act
            var result = await _workoutService.GetWorkoutsByTypeAsync(_testWorkoutType);

            // Assert
            Assert.NotNull(result);
            Assert.All(result, w => Assert.Equal(_testWorkoutType, w.Type));
        }

        [Fact]
        public async Task AddWorkoutAsync_ShouldAddWorkoutAndSaveChanges()
        {
            // Arrange
            var createDto = new CreateWorkoutDto
            {
                StartTime = 1000,
                EndTime = 2000,
                Type = _testWorkoutType,
                Reps = 10,
                UserId = _testUser.UserId
            };

            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(createDto.UserId))
                .ReturnsAsync(_testUser);

            // Act
            await _workoutService.AddWorkoutAsync(createDto);

            // Assert
            _mockWorkoutRepository.Verify(repo => repo.AddWorkoutAsync(
                It.Is<Workout>(w =>
                    w.StartTime == createDto.StartTime &&
                    w.EndTime == createDto.EndTime &&
                    w.Type == createDto.Type &&
                    w.Reps == createDto.Reps &&
                    w.UserId == createDto.UserId
                )), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateWorkoutAsync_ShouldUpdateWhenWorkoutExists()
        {
            // Arrange
            var updateDto = new UpdateWorkoutDto
            {
                WorkoutId = 1,
                StartTime = 1000,
                EndTime = 2000,
                Type = _testWorkoutType,
                Reps = 15
            };

            var existingWorkout = new Workout
            {
                WorkoutId = 1,
                StartTime = 500,
                EndTime = 1500,
                Type = _testWorkoutType,
                Reps = 10,
                UserId = _testUser.UserId,
                User = _testUser
            };

            _mockWorkoutRepository.Setup(repo => repo.GetWorkoutByIdAsync(updateDto.WorkoutId))
                .ReturnsAsync(existingWorkout);

            // Act
            await _workoutService.UpdateWorkoutAsync(updateDto);

            // Assert
            _mockWorkoutRepository.Verify(repo => repo.UpdateWorkoutAsync(
                It.Is<Workout>(w =>
                    w.WorkoutId == updateDto.WorkoutId &&
                    w.StartTime == updateDto.StartTime &&
                    w.EndTime == updateDto.EndTime &&
                    w.Type == updateDto.Type &&
                    w.Reps == updateDto.Reps
                )), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateWorkoutAsync_ShouldNotUpdateWhenWorkoutDoesNotExist()
        {
            // Arrange
            var updateDto = new UpdateWorkoutDto
            {
                WorkoutId = 999,
                Type = _testWorkoutType
            };

            _mockWorkoutRepository.Setup(repo => repo.GetWorkoutByIdAsync(updateDto.WorkoutId))
                .ReturnsAsync((Workout?)null);

            // Act
            await _workoutService.UpdateWorkoutAsync(updateDto);

            // Assert
            _mockWorkoutRepository.Verify(repo => repo.UpdateWorkoutAsync(It.IsAny<Workout>()), Times.Never);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteWorkoutAsync_ShouldDeleteAndSaveChanges()
        {
            // Arrange
            int workoutId = 1;

            // Act
            await _workoutService.DeleteWorkoutAsync(workoutId);

            // Assert
            _mockWorkoutRepository.Verify(repo => repo.DeleteWorkoutAsync(workoutId), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }
    }
} 