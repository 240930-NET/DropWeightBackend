using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using DropWeightBackend.Api.Controllers;
using DropWeightBackend.Api.Services.Interfaces;
using DropWeightBackend.Domain.Entities;

namespace DropWeightBackend.Tests
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _controller = new UserController(_mockUserService.Object);
        }

        [Fact]
        public async Task GetUserById_ShouldReturnOk_WhenUserExists()
        {
            // Arrange
            var user = new User { UserId = 1, Username = "testuser" };
            _mockUserService.Setup(service => service.GetUserByIdAsync(1))
                .ReturnsAsync(user);

            // Act
            var result = await _controller.GetUserById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(user, okResult.Value);
        }

        [Fact]
        public async Task GetUserById_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            _mockUserService.Setup(service => service.GetUserByIdAsync(999))
                .ReturnsAsync((User)null);

            // Act
            var result = await _controller.GetUserById(999);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task GetAllUsers_ShouldReturnOk_WithUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { UserId = 1, Username = "user1" },
                new User { UserId = 2, Username = "user2" }
            };
            _mockUserService.Setup(service => service.GetAllUsersAsync())
                .ReturnsAsync(users);

            // Act
            var result = await _controller.GetAllUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUsers = Assert.IsAssignableFrom<IEnumerable<User>>(okResult.Value);
            Assert.Equal(2, returnedUsers.Count());
        }

        [Fact]
        public async Task GetUserWorkoutSchedules_ShouldReturnOk_WhenUserExists()
        {
            // Arrange
            var user = new User 
            { 
                UserId = 1, 
                Username = "testuser",
                WorkoutSchedules = new List<WorkoutSchedule>
                {
                    new WorkoutSchedule { WorkoutScheduleId = 1 }
                }
            };
            _mockUserService.Setup(service => service.GetUserByIdAsync(1))
                .ReturnsAsync(user);

            // Act
            var result = await _controller.GetUserWorkoutSchedules(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var schedules = Assert.IsAssignableFrom<IEnumerable<WorkoutSchedule>>(okResult.Value);
            Assert.Single(schedules);
        }

        [Fact]
        public async Task GetUserWorkoutSchedules_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            _mockUserService.Setup(service => service.GetUserByIdAsync(999))
                .ReturnsAsync((User)null);

            // Act
            var result = await _controller.GetUserWorkoutSchedules(999);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task UpdateUser_ShouldReturnNoContent_WhenUpdateSucceeds()
        {
            // Arrange
            var user = new User { UserId = 1, Username = "testuser" };
            _mockUserService.Setup(service => service.UpdateUserAsync(user))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateUser(1, user);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateUser_ShouldReturnBadRequest_WhenIdMismatch()
        {
            // Arrange
            var user = new User { UserId = 2, Username = "testuser" };

            // Act
            var result = await _controller.UpdateUser(1, user);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("User ID mismatch.", badRequestResult.Value);
        }

        [Fact]
        public async Task DeleteUser_ShouldReturnNoContent_WhenUserExists()
        {
            // Arrange
            var user = new User { UserId = 1, Username = "testuser" };
            _mockUserService.Setup(service => service.GetUserByIdAsync(1))
                .ReturnsAsync(user);
            _mockUserService.Setup(service => service.DeleteUserAsync(1))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteUser(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteUser_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            _mockUserService.Setup(service => service.GetUserByIdAsync(999))
                .ReturnsAsync((User)null);

            // Act
            var result = await _controller.DeleteUser(999);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User not found.", notFoundResult.Value);
        }
    }
} 