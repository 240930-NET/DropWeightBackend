using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DropWeightBackend.Api.Services.Implementations;
using DropWeightBackend.Api.Services.Interfaces;
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

        public WorkoutScheduleServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockRepository = new Mock<IWorkoutScheduleRepository>();
            _mockUnitOfWork.Setup(u => u.WorkoutSchedules).Returns(_mockRepository.Object);
            _service = new WorkoutScheduleService(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetSchedulesForUserAsync_ReturnsSchedules()
        {
            // Arrange
            var userId = 1;
            var schedules = new List<WorkoutSchedule>
            {
                new WorkoutSchedule { WorkoutScheduleId = 1, UserId = userId },
                new WorkoutSchedule { WorkoutScheduleId = 2, UserId = userId }
            };
            _mockRepository.Setup(r => r.GetSchedulesForUserAsync(userId))
                           .ReturnsAsync(schedules);

            // Act
            var result = await _service.GetSchedulesForUserAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task AddScheduleAsync_CallsRepositoryAndCommits()
        {
            // Arrange
            var schedule = new WorkoutSchedule { WorkoutScheduleId = 1 };
            _mockRepository.Setup(r => r.AddScheduleAsync(schedule)).Returns(Task.CompletedTask);

            // Act
            await _service.AddScheduleAsync(schedule);

            // Assert
            _mockRepository.Verify(r => r.AddScheduleAsync(schedule), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteScheduleAsync_RemovesScheduleAndCommits()
        {
            // Arrange
            var scheduleId = 1;
            var schedule = new WorkoutSchedule { WorkoutScheduleId = scheduleId };
            _mockRepository.Setup(r => r.GetScheduleByIdAsync(scheduleId)).ReturnsAsync(schedule);

            // Act
            await _service.DeleteScheduleAsync(scheduleId);

            // Assert
            _mockRepository.Verify(r => r.DeleteScheduleAsync(scheduleId), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task GetScheduleByIdAsync_ReturnsCorrectSchedule()
        {
            // Arrange
            var scheduleId = 1;
            var schedule = new WorkoutSchedule { WorkoutScheduleId = scheduleId };
            _mockRepository.Setup(r => r.GetScheduleByIdAsync(scheduleId)).ReturnsAsync(schedule);

            // Act
            var result = await _service.GetScheduleByIdAsync(scheduleId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(scheduleId, result?.WorkoutScheduleId);
        }
    }
}
