using Moq;
using Xunit;
using DropWeightBackend.Api.Services.Implementations;
using DropWeightBackend.Domain.Entities;
using DropWeightBackend.Infrastructure.UnitOfWork;
using DropWeightBackend.Infrastructure.Repositories.Interfaces;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DropWeightBackend.Tests
{
    public class NutritionServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<INutritionRepository> _mockNutritionRepository;
        private readonly NutritionService _nutritionService;
        private readonly User _testUser;

        public NutritionServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockNutritionRepository = new Mock<INutritionRepository>();
            _mockUnitOfWork.Setup(uow => uow.Nutritions).Returns(_mockNutritionRepository.Object);
            _nutritionService = new NutritionService(_mockUnitOfWork.Object);
            
            _testUser = new User 
            { 
                UserId = 1,
                Username = "testuser"
            };
        }

        [Fact]
        public async Task GetNutritionByIdAsync_ShouldReturnNutrition_WhenExists()
        {
            // Arrange
            var expectedNutrition = new Nutrition
            {
                NutritionId = 1,
                Calories = 2000,
                Protein = 150,
                Carbohydrates = 200,
                Description = "Test nutrition entry",
                User = _testUser,
                UserId = _testUser.UserId
            };

            _mockNutritionRepository.Setup(repo => repo.GetNutritionByIdAsync(1))
                .ReturnsAsync(expectedNutrition);

            // Act
            var result = await _nutritionService.GetNutritionByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedNutrition.NutritionId, result.NutritionId);
            Assert.Equal(expectedNutrition.Calories, result.Calories);
            Assert.Equal(expectedNutrition.Protein, result.Protein);
            Assert.Equal(expectedNutrition.Carbohydrates, result.Carbohydrates);
        }

        [Fact]
        public async Task GetNutritionByIdAsync_ShouldReturnNull_WhenNotExists()
        {
            // Arrange
            _mockNutritionRepository.Setup(repo => repo.GetNutritionByIdAsync(999))
                .ReturnsAsync((Nutrition?)null);

            // Act
            var result = await _nutritionService.GetNutritionByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllNutritionsAsync_ShouldReturnAllNutritions()
        {
            // Arrange
            var expectedNutritions = new List<Nutrition>
            {
                new Nutrition 
                { 
                    NutritionId = 1, 
                    Calories = 2000, 
                    Description = "Description 1", 
                    User = _testUser,
                    UserId = _testUser.UserId
                },
                new Nutrition 
                { 
                    NutritionId = 2, 
                    Calories = 2500,
                    Description = "Description 2",
                    User = _testUser,
                    UserId = _testUser.UserId
                }
            };

            _mockNutritionRepository.Setup(repo => repo.GetAllNutritionsAsync())
                .ReturnsAsync(expectedNutritions);

            // Act
            var result = await _nutritionService.GetAllNutritionsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedNutritions.Count, result.Count());
            Assert.Equal(expectedNutritions, result);
        }

        [Fact]
        public async Task GetAllNutritionsAsync_ShouldReturnEmptyList_WhenNoNutritions()
        {
            // Arrange
            _mockNutritionRepository.Setup(repo => repo.GetAllNutritionsAsync())
                .ReturnsAsync(new List<Nutrition>());

            // Act
            var result = await _nutritionService.GetAllNutritionsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task AddNutritionAsync_ShouldCallRepositoryAndComplete()
        {
            // Arrange
            var nutrition = new Nutrition
            {
                Calories = 2000,
                Protein = 150,
                Carbohydrates = 200,
                Description = "Test nutrition entry",
                User = _testUser,
                UserId = _testUser.UserId
            };

            _mockNutritionRepository.Setup(repo => repo.AddNutritionAsync(It.IsAny<Nutrition>()))
                .Returns(Task.CompletedTask);

            // Act
            await _nutritionService.AddNutritionAsync(nutrition);

            // Assert
            _mockNutritionRepository.Verify(repo => repo.AddNutritionAsync(nutrition), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateNutritionAsync_ShouldCallRepositoryAndComplete()
        {
            // Arrange
            var nutrition = new Nutrition
            {
                NutritionId = 1,
                Calories = 2000,
                Protein = 150,
                Carbohydrates = 200,
                Description = "Test nutrition entry",
                User = _testUser,
                UserId = _testUser.UserId
            };

            _mockNutritionRepository.Setup(repo => repo.UpdateNutritionAsync(It.IsAny<Nutrition>()))
                .Returns(Task.CompletedTask);

            // Act
            await _nutritionService.UpdateNutritionAsync(nutrition);

            // Assert
            _mockNutritionRepository.Verify(repo => repo.UpdateNutritionAsync(nutrition), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteNutritionAsync_ShouldCallRepositoryAndComplete()
        {
            // Arrange
            int nutritionId = 1;

            _mockNutritionRepository.Setup(repo => repo.DeleteNutritionAsync(It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            // Act
            await _nutritionService.DeleteNutritionAsync(nutritionId);

            // Assert
            _mockNutritionRepository.Verify(repo => repo.DeleteNutritionAsync(nutritionId), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task AddNutritionAsync_ShouldThrowException_WhenNutritionIsNull()
        {
            // Arrange
            Nutrition? nullNutrition = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _nutritionService.AddNutritionAsync(nullNutrition!));
        }

        [Fact]
        public async Task UpdateNutritionAsync_ShouldThrowException_WhenNutritionIsNull()
        {
            // Arrange
            Nutrition? nullNutrition = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _nutritionService.UpdateNutritionAsync(nullNutrition!));
        }

        [Fact]
        public async Task GetNutritionsByUserIdAsync_ShouldReturnUserNutritions()
        {
            // Arrange
            var userId = 1;
            var expectedNutritions = new List<Nutrition>
            {
                new Nutrition 
                { 
                    NutritionId = 1, 
                    Calories = 2000, 
                    Description = "Description 1", 
                    User = _testUser,
                    UserId = userId
                },
                new Nutrition 
                { 
                    NutritionId = 2, 
                    Calories = 2500,
                    Description = "Description 2",
                    User = _testUser,
                    UserId = userId
                }
            };

            _mockNutritionRepository.Setup(repo => repo.GetNutritionsByUserIdAsync(userId))
                .ReturnsAsync(expectedNutritions);

            // Act
            var result = await _nutritionService.GetNutritionsByUserIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedNutritions.Count, result.Count());
            Assert.All(result, nutrition => Assert.Equal(userId, nutrition.UserId));
        }

        [Fact]
        public async Task GetNutritionsByUserIdAsync_ShouldReturnEmptyList_WhenNoNutritionsExist()
        {
            // Arrange
            var userId = 999;
            _mockNutritionRepository.Setup(repo => repo.GetNutritionsByUserIdAsync(userId))
                .ReturnsAsync(new List<Nutrition>());

            // Act
            var result = await _nutritionService.GetNutritionsByUserIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
