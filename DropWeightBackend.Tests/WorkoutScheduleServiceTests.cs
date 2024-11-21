using Xunit;
using Moq;
using DropWeightBackend.Api.Services.Implementations;
using DropWeightBackend.Domain.Entities;
using DropWeightBackend.Infrastructure.UnitOfWork;
using DropWeightBackend.Infrastructure.Repositories.Interfaces;

namespace DropWeightBackend.Tests
{
    public class WorkoutScheduleServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IWorkoutScheduleRepository> _mockRepository;
        private readonly WorkoutScheduleService _service;
        private readonly User _testUser;
        private readonly Workout _testWorkout;

        public WorkoutScheduleServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockRepository = new Mock<IWorkoutScheduleRepository>();
            _mockUnitOfWork.Setup(u => u.WorkoutSchedules).Returns(_mockRepository.Object);
            _service = new WorkoutScheduleService(_mockUnitOfWork.Object);

            // Create test user
            _testUser = new User
            {
                UserId = 1,
                Username = "testuser",
                FirstName = "Test",
                LastName = "User"
            };

            // Create test workout
            _testWorkout = new Workout
            {
                WorkoutId = 1,
                UserId = _testUser.UserId,
                User = _testUser
            };
        }

        [Fact]
        public async Task GetSchedulesForUserAsync_ShouldReturnSchedules()
        {
            // Arrange
            var userId = 1;
            var schedules = new List<WorkoutSchedule>
            {
                new WorkoutSchedule
                {
                    WorkoutScheduleId = 1,
                    UserId = userId,
                    User = _testUser,
                    WorkoutId = _testWorkout.WorkoutId,
                    Workout = _testWorkout,
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now.AddHours(1)
                },
                new WorkoutSchedule
                {
                    WorkoutScheduleId = 2,
                    UserId = userId,
                    User = _testUser,
                    WorkoutId = _testWorkout.WorkoutId,
                    Workout = _testWorkout,
                    StartTime = DateTime.Now.AddDays(1),
                    EndTime = DateTime.Now.AddDays(1).AddHours(1)
                }
            };

            _mockRepository.Setup(r => r.GetSchedulesForUserAsync(userId))
                .ReturnsAsync(schedules);

            // Act
            var result = await _service.GetSchedulesForUserAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, schedule => Assert.Equal(userId, schedule.UserId));
        }

        [Fact]
        public async Task GetSchedulesForUserAsync_ShouldReturnEmptyList_WhenNoSchedules()
        {
            // Arrange
            var userId = 1;
            _mockRepository.Setup(r => r.GetSchedulesForUserAsync(userId))
                .ReturnsAsync(new List<WorkoutSchedule>());

            // Act
            var result = await _service.GetSchedulesForUserAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetScheduleByIdAsync_ShouldReturnSchedule_WhenExists()
        {
            // Arrange
            var scheduleId = 1;
            var schedule = new WorkoutSchedule
            {
                WorkoutScheduleId = scheduleId,
                UserId = _testUser.UserId,
                User = _testUser,
                WorkoutId = _testWorkout.WorkoutId,
                Workout = _testWorkout,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddHours(1)
            };

            _mockRepository.Setup(r => r.GetScheduleByIdAsync(scheduleId))
                .ReturnsAsync(schedule);

            // Act
            var result = await _service.GetScheduleByIdAsync(scheduleId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(scheduleId, result.WorkoutScheduleId);
        }

        [Fact]
        public async Task GetScheduleByIdAsync_ShouldReturnNull_WhenNotExists()
        {
            // Arrange
            var scheduleId = 999;
            _mockRepository.Setup(r => r.GetScheduleByIdAsync(scheduleId))
                .ReturnsAsync((WorkoutSchedule?)null);

            // Act
            var result = await _service.GetScheduleByIdAsync(scheduleId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddScheduleAsync_ShouldCallRepositoryAndComplete()
        {
            // Arrange
            var schedule = new WorkoutSchedule
            {
                UserId = _testUser.UserId,
                User = _testUser,
                WorkoutId = _testWorkout.WorkoutId,
                Workout = _testWorkout,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddHours(1)
            };

            _mockRepository.Setup(r => r.AddScheduleAsync(It.IsAny<WorkoutSchedule>()))
                .Returns(Task.CompletedTask);

            // Act
            await _service.AddScheduleAsync(schedule);

            // Assert
            _mockRepository.Verify(r => r.AddScheduleAsync(schedule), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateScheduleAsync_ShouldCallRepositoryAndComplete()
        {
            // Arrange
            var schedule = new WorkoutSchedule
            {
                WorkoutScheduleId = 1,
                UserId = _testUser.UserId,
                User = _testUser,
                WorkoutId = _testWorkout.WorkoutId,
                Workout = _testWorkout,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddHours(1)
            };

            _mockRepository.Setup(r => r.UpdateScheduleAsync(It.IsAny<WorkoutSchedule>()))
                .Returns(Task.CompletedTask);

            // Act
            await _service.UpdateScheduleAsync(schedule);

            // Assert
            _mockRepository.Verify(r => r.UpdateScheduleAsync(schedule), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteScheduleAsync_ShouldCallRepositoryAndComplete()
        {
            // Arrange
            var scheduleId = 1;

            _mockRepository.Setup(r => r.DeleteScheduleAsync(It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            // Act
            await _service.DeleteScheduleAsync(scheduleId);

            // Assert
            _mockRepository.Verify(r => r.DeleteScheduleAsync(scheduleId), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }
    }
}
