using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using DropWeightBackend.Api.Controllers;
using DropWeightBackend.Api.Services.Interfaces;
using DropWeightBackend.Domain.Entities;

namespace DropWeightBackend.Tests
{
    public class NutritionControllerTests
    {
        private readonly Mock<INutritionService> _mockNutritionService;
        private readonly NutritionController _controller;
        private readonly User _testUser;

        public NutritionControllerTests()
        {
            _mockNutritionService = new Mock<INutritionService>();
            _controller = new NutritionController(_mockNutritionService.Object);
            
            _testUser = new User
            {
                UserId = 1,
                Username = "testuser"
            };
        }

        [Fact]
        public async Task GetAllNutrition_ShouldReturnOkResult_WithNutritions()
        {
            // Arrange
            var nutritions = new List<Nutrition>
            {
                new Nutrition { 
                    NutritionId = 1, 
                    UserId = _testUser.UserId,
                    Description = "Test Nutrition 1",
                    ServingSize = 100,
                    Calories = 200,
                    Date = DateTime.Now
                },
                new Nutrition { 
                    NutritionId = 2, 
                    UserId = _testUser.UserId,
                    Description = "Test Nutrition 2",
                    ServingSize = 150,
                    Calories = 300,
                    Date = DateTime.Now
                }
            };

            _mockNutritionService.Setup(service => service.GetAllNutritionsAsync())
                .ReturnsAsync(nutritions);

            // Act
            var result = await _controller.GetAllNutrition();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedNutritions = Assert.IsAssignableFrom<IEnumerable<Nutrition>>(okResult.Value);
            Assert.Equal(2, returnedNutritions.Count());
        }

        [Fact]
        public async Task GetNutritionById_ShouldReturnOkResult_WhenNutritionExists()
        {
            // Arrange
            var nutrition = new Nutrition 
            { 
                NutritionId = 1, 
                UserId = _testUser.UserId,
                Description = "Test Nutrition",
                ServingSize = 100,
                Calories = 200,
                Date = DateTime.Now
            };

            _mockNutritionService.Setup(service => service.GetNutritionByIdAsync(1))
                .ReturnsAsync(nutrition);

            // Act
            var result = await _controller.GetNutritionById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedNutrition = Assert.IsType<Nutrition>(okResult.Value);
            Assert.Equal(1, returnedNutrition.NutritionId);
        }

        [Fact]
        public async Task GetNutritionById_ShouldReturnNotFound_WhenNutritionDoesNotExist()
        {
            // Arrange
            _mockNutritionService.Setup(service => service.GetNutritionByIdAsync(999))
                .ReturnsAsync((Nutrition?)null);

            // Act
            var result = await _controller.GetNutritionById(999);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetAllNutrition_ByUserId_ShouldReturnOkResult_WithUserNutritions()
        {
            // Arrange
            var userId = 1;
            var nutritions = new List<Nutrition>
            {
                new Nutrition { 
                    NutritionId = 1, 
                    UserId = userId,
                    Description = "Test Nutrition 1",
                    ServingSize = 100,
                    Calories = 200,
                    Date = DateTime.Now
                },
                new Nutrition { 
                    NutritionId = 2, 
                    UserId = userId,
                    Description = "Test Nutrition 2",
                    ServingSize = 150,
                    Calories = 300,
                    Date = DateTime.Now
                }
            };

            _mockNutritionService.Setup(service => service.GetNutritionsByUserIdAsync(userId))
                .ReturnsAsync(nutritions);

            // Act
            var result = await _controller.GetAllNutrition(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedNutritions = Assert.IsAssignableFrom<IEnumerable<Nutrition>>(okResult.Value);
            Assert.Equal(2, returnedNutritions.Count());
            Assert.All(returnedNutritions, n => Assert.Equal(userId, n.UserId));
        }

        [Fact]
        public async Task CreateNutrition_ShouldReturnOkResult()
        {
            // Arrange
            var nutrition = new Nutrition 
            { 
                NutritionId = 1, 
                UserId = _testUser.UserId,
                Description = "Test Nutrition",
                ServingSize = 100,
                Calories = 200,
                Date = DateTime.Now
            };

            _mockNutritionService.Setup(service => service.AddNutritionAsync(nutrition))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateNutrition(nutrition);

            // Assert
            Assert.IsType<OkResult>(result.Result);
            _mockNutritionService.Verify(service => service.AddNutritionAsync(nutrition), Times.Once);
        }

        [Fact]
        public async Task UpdateNutrition_ShouldReturnOkResult()
        {
            // Arrange
            var nutrition = new Nutrition 
            { 
                NutritionId = 1, 
                UserId = _testUser.UserId,
                Description = "Test Nutrition",
                ServingSize = 100,
                Calories = 200,
                Date = DateTime.Now
            };

            _mockNutritionService.Setup(service => service.UpdateNutritionAsync(nutrition))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateNutrition(nutrition);

            // Assert
            Assert.IsType<OkResult>(result);
            _mockNutritionService.Verify(service => service.UpdateNutritionAsync(nutrition), Times.Once);
        }

        [Fact]
        public async Task DeleteNutrition_ShouldReturnOkResult_WhenNutritionExists()
        {
            // Arrange
            var nutrition = new Nutrition 
            { 
                NutritionId = 1, 
                UserId = _testUser.UserId,
                Description = "Test Nutrition",
                ServingSize = 100,
                Calories = 200,
                Date = DateTime.Now
            };

            _mockNutritionService.Setup(service => service.GetNutritionByIdAsync(1))
                .ReturnsAsync(nutrition);
            _mockNutritionService.Setup(service => service.DeleteNutritionAsync(1))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteNutrition(1);

            // Assert
            Assert.IsType<OkResult>(result);
            _mockNutritionService.Verify(service => service.DeleteNutritionAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteNutrition_ShouldReturnNotFound_WhenNutritionDoesNotExist()
        {
            // Arrange
            _mockNutritionService.Setup(service => service.GetNutritionByIdAsync(999))
                .ReturnsAsync((Nutrition?)null);

            // Act
            var result = await _controller.DeleteNutrition(999);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Nutrition not found.", notFoundResult.Value);
            _mockNutritionService.Verify(service => service.DeleteNutritionAsync(999), Times.Never);
        }
    }
} 