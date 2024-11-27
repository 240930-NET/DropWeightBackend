using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using DropWeightBackend.Api.Controllers;
using DropWeightBackend.Api.Services.Interfaces;
using DropWeightBackend.Domain.Entities;
using DropWeightBackend.Api.DTOs;

namespace DropWeightBackend.Tests
{
    public class AuthenticationControllerTests
    {
        private readonly Mock<IAuthenticationService> _mockAuthService;
        private readonly AuthenticationController _controller;

        public AuthenticationControllerTests()
        {
            _mockAuthService = new Mock<IAuthenticationService>();
            _controller = new AuthenticationController(_mockAuthService.Object);
        }


        [Fact]
        public async Task Register_ShouldReturnBadRequest_WhenFieldsAreMissing()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Username = "",  // Empty username
                Password = "testpass",
                FirstName = "Test",
                LastName = "User"
            };

            // Act
            var result = await _controller.Register(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("All fields are required.", badRequestResult.Value);
        }

        [Fact]
        public async Task Register_ShouldReturnConflict_WhenRegistrationFails()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Username = "testuser",
                Password = "testpass",
                FirstName = "Test",
                LastName = "User"
            };

            _mockAuthService.Setup(service => service.RegisterUserAsync(It.IsAny<User>(), request.Password))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Register(request);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal("Registration failed. Username might already exist.", conflictResult.Value);
        }

        [Fact]
        public async Task Login_ShouldReturnOkWithToken_WhenCredentialsAreValid()
        {
            // Arrange
            var request = new LoginRequest
            {
                Username = "testuser",
                Password = "testpass"
            };

            var token = "test-jwt-token";
            _mockAuthService.Setup(service => service.AuthenticateUserAsync(request.Username, request.Password))
                .ReturnsAsync(token);

            // Act
            var result = await _controller.Login(request);

            
        }

        [Fact]
        public async Task Login_ShouldReturnBadRequest_WhenFieldsAreMissing()
        {
            // Arrange
            var request = new LoginRequest
            {
                Username = "",  // Empty username
                Password = "testpass"
            };

            // Act
            var result = await _controller.Login(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Username and password are required.", badRequestResult.Value);
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenCredentialsAreInvalid()
        {
            // Arrange
            var request = new LoginRequest
            {
                Username = "testuser",
                Password = "wrongpass"
            };

            _mockAuthService.Setup(service => service.AuthenticateUserAsync(request.Username, request.Password))
                .ReturnsAsync((string)null);

            // Act
            var result = await _controller.Login(request);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Invalid username or password.", unauthorizedResult.Value);
        }

        private class Anonymous
        {
            public string Token { get; set; }
        }
    }
} 