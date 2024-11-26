using Xunit;
using Microsoft.EntityFrameworkCore;
using DropWeightBackend.Infrastructure.Data;
using DropWeightBackend.Infrastructure.Repositories.Implementations;
using DropWeightBackend.Domain.Entities;
using DropWeightBackend.Domain.Enums;

namespace DropWeightBackend.Tests.Repositories
{
    public class GoalRepositoryTests : IDisposable
    {
        private readonly DbContextOptions<DropWeightContext> _options;
        private readonly DropWeightContext _context;
        private readonly GoalRepository _repository;
        private readonly User _testUser;

        public GoalRepositoryTests()
        {
            var dbName = Guid.NewGuid().ToString();
            _options = new DbContextOptionsBuilder<DropWeightContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            _context = new DropWeightContext(_options);
            _repository = new GoalRepository(_context);

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
        public async Task GetGoalById_ShouldReturnGoal_WhenExists()
        {
            // Arrange
            var goal = new Goal
            {
                GoalId = 1,
                Type = GoalType.Weight,
                Description = "Test Goal",
                StartingValue = 100,
                CurrentValue = 90,
                TargetValue = 80,
                UserId = _testUser.UserId
            };
            await _context.Goals.AddAsync(goal);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetGoalById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Goal", result.Description);
        }

        [Fact]
        public async Task GetAllGoals_ShouldReturnAllGoals()
        {
            // Arrange
            var goals = new List<Goal>
            {
                new Goal
                {
                    GoalId = 2,
                    Type = GoalType.Weight,
                    Description = "Goal 1",
                    StartingValue = 100,
                    CurrentValue = 90,
                    TargetValue = 80,
                    UserId = _testUser.UserId
                },
                new Goal
                {
                    GoalId = 3,
                    Type = GoalType.Strength,
                    Description = "Goal 2",
                    StartingValue = 50,
                    CurrentValue = 60,
                    TargetValue = 70,
                    UserId = _testUser.UserId
                }
            };
            await _context.Goals.AddRangeAsync(goals);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllGoals();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task AddGoal_ShouldAddGoal()
        {
            // Arrange
            var goal = new Goal
            {
                GoalId = 4,
                Type = GoalType.Weight,
                Description = "New Goal",
                StartingValue = 100,
                CurrentValue = 90,
                TargetValue = 80,
                UserId = _testUser.UserId
            };

            // Act
            var result = await _repository.AddGoal(goal);

            // Assert
            var addedGoal = await _context.Goals.FindAsync(4);
            Assert.NotNull(addedGoal);
            Assert.Equal("New Goal", addedGoal.Description);
            Assert.Equal(goal, result);
        }

        [Fact]
        public async Task UpdateGoal_ShouldUpdateGoal()
        {
            // Arrange
            var goal = new Goal
            {
                GoalId = 5,
                Type = GoalType.Weight,
                Description = "Original Goal",
                StartingValue = 100,
                CurrentValue = 90,
                TargetValue = 80,
                UserId = _testUser.UserId
            };
            await _context.Goals.AddAsync(goal);
            await _context.SaveChangesAsync();

            // Act
            goal.Description = "Updated Goal";
            var result = await _repository.UpdateGoal(goal);

            // Assert
            var updatedGoal = await _context.Goals.FindAsync(5);
            Assert.NotNull(updatedGoal);
            Assert.Equal("Updated Goal", updatedGoal.Description);
            Assert.Equal(goal, result);
        }

        [Fact]
        public async Task DeleteGoal_ShouldDeleteGoal()
        {
            // Arrange
            var goal = new Goal
            {
                GoalId = 6,
                Type = GoalType.Weight,
                Description = "To Delete",
                StartingValue = 100,
                CurrentValue = 90,
                TargetValue = 80,
                UserId = _testUser.UserId
            };
            await _context.Goals.AddAsync(goal);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteGoal(goal);

            // Assert
            var deletedGoal = await _context.Goals.FindAsync(6);
            Assert.Null(deletedGoal);
        }
    }
} 