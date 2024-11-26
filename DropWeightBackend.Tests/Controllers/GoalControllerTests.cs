using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using DropWeightBackend.Api.Controllers;
using DropWeightBackend.Api.Services.Interfaces;
using DropWeightBackend.Domain.Entities;
using DropWeightBackend.Api.DTOs;
using DropWeightBackend.Domain.Enums;

namespace DropWeightBackend.Tests
{
    public class GoalControllerTests
    {
        private readonly Mock<IGoalService> _mockGoalService;
        private readonly GoalController _controller;

        public GoalControllerTests()
        {
            _mockGoalService = new Mock<IGoalService>();
            _controller = new GoalController(_mockGoalService.Object);
        }

        [Fact]
        public async Task GetAllGoals_ShouldReturnOk_WhenGoalsExist()
        {
            // Arrange
            var goals = new List<Goal>
            {
                new Goal { GoalId = 1, Type = GoalType.Weight },
                new Goal { GoalId = 2, Type = GoalType.Strength }
            };
            _mockGoalService.Setup(service => service.GetAllGoals())
                .ReturnsAsync(goals);

            // Act
            var result = await _controller.GetAllGoals();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedGoals = Assert.IsType<List<Goal>>(okResult.Value);
            Assert.Equal(2, returnedGoals.Count);
        }

        [Fact]
        public async Task GetAllGoals_ShouldReturnBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            _mockGoalService.Setup(service => service.GetAllGoals())
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.GetAllGoals();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Test exception", badRequestResult.Value);
        }

        [Fact]
        public async Task GetGoalById_ShouldReturnOk_WhenGoalExists()
        {
            // Arrange
            var goal = new Goal { GoalId = 1, Type = GoalType.Weight };
            _mockGoalService.Setup(service => service.GetGoalById(1))
                .ReturnsAsync(goal);

            // Act
            var result = await _controller.GetGoalById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedGoal = Assert.IsType<Goal>(okResult.Value);
            Assert.Equal(1, returnedGoal.GoalId);
        }

        [Fact]
        public async Task GetGoalById_ShouldReturnNotFound_WhenGoalDoesNotExist()
        {
            // Arrange
            _mockGoalService.Setup(service => service.GetGoalById(999))
                .ReturnsAsync((Goal)null);

            // Act
            var result = await _controller.GetGoalById(999);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Goal with id: 999 not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task GetGoalById_ShouldReturnBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            _mockGoalService.Setup(service => service.GetGoalById(1))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.GetGoalById(1);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Test exception", badRequestResult.Value);
        }

        [Fact]
        public async Task AddGoal_ShouldReturnOk_WhenGoalIsValid()
        {
            // Arrange
            var goalDto = new GoalDto
            {
                Type = GoalType.Weight,
                StartingValue = 100,
                CurrentValue = 90,
                TargetValue = 80
            };

            var addedGoal = new Goal { GoalId = 1, Type = GoalType.Weight };
            _mockGoalService.Setup(service => service.AddGoal(goalDto))
                .ReturnsAsync(addedGoal);

            // Act
            var result = await _controller.AddGoal(goalDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(goalDto, okResult.Value);
        }

        [Fact]
        public async Task AddGoal_ShouldReturnBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var goalDto = new GoalDto { Type = GoalType.Weight };
            _mockGoalService.Setup(service => service.AddGoal(goalDto))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.AddGoal(goalDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Test exception", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateGoal_ShouldReturnOk_WhenGoalIsValid()
        {
            // Arrange
            var goalDto = new GoalDto
            {
                Type = GoalType.Weight,
                StartingValue = 100,
                CurrentValue = 90,
                TargetValue = 80
            };

            var updatedGoal = new Goal { GoalId = 1, Type = GoalType.Weight };
            _mockGoalService.Setup(service => service.UpdateGoal(goalDto, 1))
                .ReturnsAsync(updatedGoal);

            // Act
            var result = await _controller.UpdateGoal(goalDto, 1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(goalDto, okResult.Value);
        }

        [Fact]
        public async Task UpdateGoal_ShouldReturnBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var goalDto = new GoalDto { Type = GoalType.Weight };
            _mockGoalService.Setup(service => service.UpdateGoal(goalDto, 1))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.UpdateGoal(goalDto, 1);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Test exception", badRequestResult.Value);
        }

        [Fact]
        public async Task DeleteGoal_ShouldReturnOk_WhenGoalExists()
        {
            // Arrange
            _mockGoalService.Setup(service => service.DeleteGoal(1))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteGoal(1);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteGoal_ShouldReturnBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            _mockGoalService.Setup(service => service.DeleteGoal(1))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.DeleteGoal(1);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Test exception", badRequestResult.Value);
        }
    }
} 