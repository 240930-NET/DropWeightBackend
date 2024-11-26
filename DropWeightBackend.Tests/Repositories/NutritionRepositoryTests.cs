using Xunit;
using Microsoft.EntityFrameworkCore;
using DropWeightBackend.Infrastructure.Data;
using DropWeightBackend.Infrastructure.Repositories.Implementations;
using DropWeightBackend.Domain.Entities;

namespace DropWeightBackend.Tests.Repositories
{
    public class NutritionRepositoryTests : IDisposable
    {
        private readonly DbContextOptions<DropWeightContext> _options;
        private readonly DropWeightContext _context;
        private readonly NutritionRepository _repository;
        private readonly User _testUser;

        public NutritionRepositoryTests()
        {
            var dbName = Guid.NewGuid().ToString();
            _options = new DbContextOptionsBuilder<DropWeightContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            _context = new DropWeightContext(_options);
            _repository = new NutritionRepository(_context);

            // Create test user
            _testUser = new User
            {
                UserId = 1,
                Username = "testuser",
                FirstName = "Test",
                LastName = "User",
                PasswordHash = "hash",
                PasswordSalt = "salt"
            };
            _context.Users.Add(_testUser);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task GetNutritionByIdAsync_ShouldReturnNutrition_WhenExists()
        {
            // Arrange
            var nutrition = new Nutrition
            {
                NutritionId = 1,
                Description = "Test Nutrition",
                ServingSize = 100,
                Calories = 200,
                Date = DateTime.Now,
                UserId = _testUser.UserId
            };
            await _context.Nutritions.AddAsync(nutrition);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetNutritionByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Nutrition", result.Description);
        }

        [Fact]
        public async Task GetAllNutritionsAsync_ShouldReturnAllNutritions()
        {
            // Arrange
            var nutritions = new List<Nutrition>
            {
                new Nutrition
                {
                    NutritionId = 2,
                    Description = "Nutrition 1",
                    ServingSize = 100,
                    Calories = 200,
                    Date = DateTime.Now,
                    UserId = _testUser.UserId
                },
                new Nutrition
                {
                    NutritionId = 3,
                    Description = "Nutrition 2",
                    ServingSize = 150,
                    Calories = 300,
                    Date = DateTime.Now,
                    UserId = _testUser.UserId
                }
            };
            await _context.Nutritions.AddRangeAsync(nutritions);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllNutritionsAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetNutritionsByUserIdAsync_ShouldReturnUserNutritions()
        {
            // Arrange
            var nutritions = new List<Nutrition>
            {
                new Nutrition
                {
                    NutritionId = 4,
                    Description = "User Nutrition 1",
                    ServingSize = 100,
                    Calories = 200,
                    Date = DateTime.Now,
                    UserId = _testUser.UserId
                },
                new Nutrition
                {
                    NutritionId = 5,
                    Description = "User Nutrition 2",
                    ServingSize = 150,
                    Calories = 300,
                    Date = DateTime.Now,
                    UserId = _testUser.UserId
                }
            };
            await _context.Nutritions.AddRangeAsync(nutritions);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetNutritionsByUserIdAsync(_testUser.UserId);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.All(result, n => Assert.Equal(_testUser.UserId, n.UserId));
        }

        [Fact]
        public async Task AddNutritionAsync_ShouldAddNutrition()
        {
            // Arrange
            var nutrition = new Nutrition
            {
                NutritionId = 6,
                Description = "New Nutrition",
                ServingSize = 100,
                Calories = 200,
                Date = DateTime.Now,
                UserId = _testUser.UserId
            };

            // Act
            await _repository.AddNutritionAsync(nutrition);

            // Assert
            var addedNutrition = await _context.Nutritions.FindAsync(6);
            Assert.NotNull(addedNutrition);
            Assert.Equal("New Nutrition", addedNutrition.Description);
        }

        [Fact]
        public async Task UpdateNutritionAsync_ShouldUpdateNutrition()
        {
            // Arrange
            var nutrition = new Nutrition
            {
                NutritionId = 7,
                Description = "Original Nutrition",
                ServingSize = 100,
                Calories = 200,
                Date = DateTime.Now,
                UserId = _testUser.UserId
            };
            await _context.Nutritions.AddAsync(nutrition);
            await _context.SaveChangesAsync();

            // Act
            nutrition.Description = "Updated Nutrition";
            await _repository.UpdateNutritionAsync(nutrition);

            // Assert
            var updatedNutrition = await _context.Nutritions.FindAsync(7);
            Assert.NotNull(updatedNutrition);
            Assert.Equal("Updated Nutrition", updatedNutrition.Description);
        }

        [Fact]
        public async Task DeleteNutritionAsync_ShouldDeleteNutrition()
        {
            // Arrange
            var nutrition = new Nutrition
            {
                NutritionId = 8,
                Description = "To Delete",
                ServingSize = 100,
                Calories = 200,
                Date = DateTime.Now,
                UserId = _testUser.UserId
            };
            await _context.Nutritions.AddAsync(nutrition);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteNutritionAsync(8);

            // Assert
            var deletedNutrition = await _context.Nutritions.FindAsync(8);
            Assert.Null(deletedNutrition);
        }

        [Fact]
        public async Task DeleteNutritionAsync_ShouldNotThrow_WhenNutritionDoesNotExist()
        {
            // Act & Assert
            await _repository.DeleteNutritionAsync(999); // Should not throw
        }
    }
} 