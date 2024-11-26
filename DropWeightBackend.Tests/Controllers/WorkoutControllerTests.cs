using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using DropWeightBackend.Api.Controllers;
using DropWeightBackend.Api.Services.Interfaces;
using DropWeightBackend.Api.DTOs;
using DropWeightBackend.Domain.Enums;

namespace DropWeightBackend.Tests
{
    public class WorkoutControllerTests
    {
        private readonly Mock<IWorkoutService> _mockWorkoutService;
        private readonly WorkoutController _controller;

        public WorkoutControllerTests()
        {
            _mockWorkoutService = new Mock<IWorkoutService>();
            _controller = new WorkoutController(_mockWorkoutService.Object);
        }

        [Fact]
        public async Task GetWorkoutById_ShouldReturnOk_WhenWorkoutExists()
        {
            // Arrange
            var workoutDto = new WorkoutDto { WorkoutId = 1 };
            _mockWorkoutService.Setup(service => service.GetWorkoutByIdAsync(1))
                .ReturnsAsync(workoutDto);

            // Act
            var result = await _controller.GetWorkoutById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedWorkout = Assert.IsType<WorkoutDto>(okResult.Value);
            Assert.Equal(workoutDto.WorkoutId, returnedWorkout.WorkoutId);
        }

        [Fact]
        public async Task GetWorkoutById_ShouldReturnNotFound_WhenWorkoutDoesNotExist()
        {
            // Arrange
            _mockWorkoutService.Setup(service => service.GetWorkoutByIdAsync(999))
                .ReturnsAsync((WorkoutDto)null);

            // Act
            var result = await _controller.GetWorkoutById(999);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetAllWorkouts_ShouldReturnOk_WithWorkouts()
        {
            // Arrange
            var workouts = new List<WorkoutDto>
            {
                new WorkoutDto { WorkoutId = 1 },
                new WorkoutDto { WorkoutId = 2 }
            };
            _mockWorkoutService.Setup(service => service.GetAllWorkoutsAsync())
                .ReturnsAsync(workouts);

            // Act
            var result = await _controller.GetAllWorkouts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedWorkouts = Assert.IsAssignableFrom<IEnumerable<WorkoutDto>>(okResult.Value);
            Assert.Equal(2, returnedWorkouts.Count());
        }

        [Fact]
        public async Task GetWorkoutsByType_ShouldReturnOk_WithWorkouts()
        {
            // Arrange
            var workoutType = (WorkoutType)0;
            var workouts = new List<WorkoutDto>
            {
                new WorkoutDto { WorkoutId = 1, Type = workoutType }
            };
            _mockWorkoutService.Setup(service => service.GetWorkoutsByTypeAsync(workoutType))
                .ReturnsAsync(workouts);

            // Act
            var result = await _controller.GetWorkoutsByType(workoutType);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedWorkouts = Assert.IsAssignableFrom<IEnumerable<WorkoutDto>>(okResult.Value);
            Assert.All(returnedWorkouts, w => Assert.Equal(workoutType, w.Type));
        }

        [Fact]
        public async Task AddWorkout_ShouldReturnCreatedAtAction_WhenModelIsValid()
        {
            // Arrange
            var createDto = new CreateWorkoutDto { UserId = 1 };
            _mockWorkoutService.Setup(service => service.AddWorkoutAsync(createDto))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddWorkout(createDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(WorkoutController.GetWorkoutById), createdAtActionResult.ActionName);
            Assert.Equal(createDto.UserId, createdAtActionResult.RouteValues["id"]);
        }

        [Fact]
        public async Task AddWorkout_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var createDto = new CreateWorkoutDto();
            _controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await _controller.AddWorkout(createDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateWorkout_ShouldReturnNoContent_WhenModelIsValid()
        {
            // Arrange
            var updateDto = new UpdateWorkoutDto { WorkoutId = 1 };
            _mockWorkoutService.Setup(service => service.UpdateWorkoutAsync(updateDto))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateWorkout(updateDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateWorkout_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var updateDto = new UpdateWorkoutDto();
            _controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await _controller.UpdateWorkout(updateDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task DeleteWorkout_ShouldReturnNoContent()
        {
            // Arrange
            _mockWorkoutService.Setup(service => service.DeleteWorkoutAsync(1))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteWorkout(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
} 