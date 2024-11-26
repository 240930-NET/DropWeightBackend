using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using DropWeightBackend.API.Controllers;
using DropWeightBackend.Api.Services.Interfaces;
using DropWeightBackend.Domain.Entities;
using DropWeightBackend.API.DTOs;
using DropWeightBackend.Domain.Enums;

namespace DropWeightBackend.Tests.Controllers
{
    public class WorkoutScheduleControllerTests
    {
        private readonly Mock<IWorkoutScheduleService> _mockService;
        private readonly WorkoutScheduleController _controller;

        public WorkoutScheduleControllerTests()
        {
            _mockService = new Mock<IWorkoutScheduleService>();
            _controller = new WorkoutScheduleController(_mockService.Object);
        }

        [Fact]
        public async Task GetSchedulesForUser_ShouldReturnOk_WhenSchedulesExist()
        {
            // Arrange
            var userId = 1;
            var workout = new Workout { Type = WorkoutType.Run, Reps = 10 };
            var schedules = new List<WorkoutSchedule>
            {
                new WorkoutSchedule 
                { 
                    WorkoutScheduleId = 1, 
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now.AddHours(1),
                    WorkoutId = 1,
                    Workout = workout,
                    UserId = userId
                }
            };

            _mockService.Setup(s => s.GetSchedulesForUserAsync(userId))
                .ReturnsAsync(schedules);

            // Act
            var result = await _controller.GetSchedulesForUser(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsAssignableFrom<IEnumerable<WorkoutScheduleResponse>>(okResult.Value);
            var schedule = response.First();
            Assert.Equal(workout.Type.ToString(), schedule.WorkoutType);
            Assert.Equal(workout.Reps, schedule.Reps);
        }

        [Fact]
        public async Task GetSchedulesForUser_ShouldReturnNotFound_WhenNoSchedules()
        {
            // Arrange
            var userId = 1;
            _mockService.Setup(s => s.GetSchedulesForUserAsync(userId))
                .ReturnsAsync(new List<WorkoutSchedule>());

            // Act
            var result = await _controller.GetSchedulesForUser(userId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No workout schedules found for this user.", notFoundResult.Value);
        }

        [Fact]
        public async Task GetScheduleById_ShouldReturnOk_WhenScheduleExists()
        {
            // Arrange
            var scheduleId = 1;
            var workout = new Workout { Type = WorkoutType.Run, Reps = 10 };
            var schedule = new WorkoutSchedule
            {
                WorkoutScheduleId = scheduleId,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddHours(1),
                WorkoutId = 1,
                Workout = workout,
                UserId = 1
            };

            _mockService.Setup(s => s.GetScheduleByIdAsync(scheduleId))
                .ReturnsAsync(schedule);

            // Act
            var result = await _controller.GetScheduleById(scheduleId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<WorkoutScheduleResponse>(okResult.Value);
            Assert.Equal(workout.Type.ToString(), response.WorkoutType);
            Assert.Equal(workout.Reps, response.Reps);
        }

        [Fact]
        public async Task GetScheduleById_ShouldReturnNotFound_WhenScheduleDoesNotExist()
        {
            // Arrange
            _mockService.Setup(s => s.GetScheduleByIdAsync(999))
                .ReturnsAsync((WorkoutSchedule)null);

            // Act
            var result = await _controller.GetScheduleById(999);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Workout schedule not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task AddSchedule_ShouldReturnCreatedAtAction_WhenModelIsValid()
        {
            // Arrange
            var request = new WorkoutScheduleRequest
            {
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddHours(1),
                WorkoutId = 1,
                UserId = 1
            };

            // Act
            var result = await _controller.AddSchedule(request);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var response = Assert.IsType<WorkoutScheduleResponse>(createdAtActionResult.Value);
            Assert.Equal(request.WorkoutId, response.WorkoutId);
            _mockService.Verify(s => s.AddScheduleAsync(It.IsAny<WorkoutSchedule>()), Times.Once);
        }

        [Fact]
        public async Task AddSchedule_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await _controller.AddSchedule(new WorkoutScheduleRequest());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateSchedule_ShouldReturnNoContent_WhenUpdateSucceeds()
        {
            // Arrange
            var scheduleId = 1;
            var existingSchedule = new WorkoutSchedule { WorkoutScheduleId = scheduleId };
            var request = new WorkoutScheduleRequest
            {
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddHours(1),
                WorkoutId = 1,
                UserId = 1
            };

            _mockService.Setup(s => s.GetScheduleByIdAsync(scheduleId))
                .ReturnsAsync(existingSchedule);

            // Act
            var result = await _controller.UpdateSchedule(scheduleId, request);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockService.Verify(s => s.UpdateScheduleAsync(It.IsAny<WorkoutSchedule>()), Times.Once);
        }

        [Fact]
        public async Task UpdateSchedule_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await _controller.UpdateSchedule(1, new WorkoutScheduleRequest());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateSchedule_ShouldReturnNotFound_WhenScheduleDoesNotExist()
        {
            // Arrange
            _mockService.Setup(s => s.GetScheduleByIdAsync(999))
                .ReturnsAsync((WorkoutSchedule)null);

            var request = new WorkoutScheduleRequest
            {
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddHours(1),
                WorkoutId = 1,
                UserId = 1
            };

            // Act
            var result = await _controller.UpdateSchedule(999, request);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Workout schedule not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task DeleteSchedule_ShouldReturnNoContent_WhenDeleteSucceeds()
        {
            // Arrange
            var scheduleId = 1;
            var existingSchedule = new WorkoutSchedule { WorkoutScheduleId = scheduleId };

            _mockService.Setup(s => s.GetScheduleByIdAsync(scheduleId))
                .ReturnsAsync(existingSchedule);

            // Act
            var result = await _controller.DeleteSchedule(scheduleId);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockService.Verify(s => s.DeleteScheduleAsync(scheduleId), Times.Once);
        }

        [Fact]
        public async Task DeleteSchedule_ShouldReturnNotFound_WhenScheduleDoesNotExist()
        {
            // Arrange
            _mockService.Setup(s => s.GetScheduleByIdAsync(999))
                .ReturnsAsync((WorkoutSchedule)null);

            // Act
            var result = await _controller.DeleteSchedule(999);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Workout schedule not found.", notFoundResult.Value);
        }
    }
} 