using Xunit;
using Microsoft.EntityFrameworkCore;
using DropWeightBackend.Infrastructure.Data;
using DropWeightBackend.Infrastructure.Repositories.Implementations;
using DropWeightBackend.Domain.Entities;
using DropWeightBackend.Domain.Enums;

namespace DropWeightBackend.Tests.Repositories
{
    public class UserRepositoryTests : IDisposable
    {
        private readonly DbContextOptions<DropWeightContext> _options;
        private readonly DropWeightContext _context;
        private readonly UserRepository _repository;

        public UserRepositoryTests()
        {
            var dbName = Guid.NewGuid().ToString();
            _options = new DbContextOptionsBuilder<DropWeightContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            _context = new DropWeightContext(_options);
            _repository = new UserRepository(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUserWithRelatedData_WhenExists()
        {
            // Arrange
            var user = new User 
            { 
                UserId = 1, 
                Username = "testuser",
                FirstName = "Test",
                LastName = "User",
                PasswordHash = "hash",
                PasswordSalt = "salt"
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Add related data
            var workout = new Workout { WorkoutId = 1, UserId = user.UserId, Type = WorkoutType.Run };
            var goal = new Goal { GoalId = 1, UserId = user.UserId, Type = GoalType.Custom, Description = "Test Goal" };
            var nutrition = new Nutrition { NutritionId = 1, UserId = user.UserId, Description = "Test Nutrition", ServingSize = 100, Calories = 200, Date = DateTime.Now };
            var schedule = new WorkoutSchedule { WorkoutScheduleId = 1, UserId = user.UserId, StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1) };

            await _context.Workouts.AddAsync(workout);
            await _context.Goals.AddAsync(goal);
            await _context.Nutritions.AddAsync(nutrition);
            await _context.WorkoutSchedules.AddAsync(schedule);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("testuser", result.Username);
            Assert.NotNull(result.Workouts);
            Assert.NotNull(result.Goals);
            Assert.NotNull(result.Nutritions);
            Assert.NotNull(result.WorkoutSchedules);
            Assert.Single(result.Workouts);
            Assert.Single(result.Goals);
            Assert.Single(result.Nutritions);
            Assert.Single(result.WorkoutSchedules);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Act
            var result = await _repository.GetUserByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserByUsernameAsync_ShouldReturnUserWithRelatedData_WhenExists()
        {
            // Arrange
            var user = new User 
            { 
                UserId = 2, 
                Username = "testuser2",
                FirstName = "Test",
                LastName = "User",
                PasswordHash = "hash",
                PasswordSalt = "salt"
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Add related data
            var workout = new Workout { WorkoutId = 2, UserId = user.UserId, Type = WorkoutType.Lift };
            await _context.Workouts.AddAsync(workout);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserByUsernameAsync("testuser2");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("testuser2", result.Username);
            Assert.NotNull(result.Workouts);
            Assert.Single(result.Workouts);
        }

        [Fact]
        public async Task GetUserByUsernameAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Act
            var result = await _repository.GetUserByUsernameAsync("nonexistent");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnAllUsersWithRelatedData()
        {
            // Arrange
            var users = new List<User>
            {
                new User { UserId = 3, Username = "user3", FirstName = "Test", LastName = "User", PasswordHash = "hash", PasswordSalt = "salt" },
                new User { UserId = 4, Username = "user4", FirstName = "Test", LastName = "User", PasswordHash = "hash", PasswordSalt = "salt" }
            };
            await _context.Users.AddRangeAsync(users);

            // Add related data
            var workout = new Workout { WorkoutId = 3, UserId = users[0].UserId, Type = WorkoutType.Squat };
            await _context.Workouts.AddAsync(workout);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllUsersAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, u => u.Workouts.Any());
        }

        [Fact]
        public async Task AddUserAsync_ShouldAddUser()
        {
            // Arrange
            var user = new User 
            { 
                UserId = 5, 
                Username = "newuser",
                FirstName = "Test",
                LastName = "User",
                PasswordHash = "hash",
                PasswordSalt = "salt"
            };

            // Act
            await _repository.AddUserAsync(user);

            // Assert
            var addedUser = await _context.Users.FindAsync(5);
            Assert.NotNull(addedUser);
            Assert.Equal("newuser", addedUser.Username);
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldUpdateUser()
        {
            // Arrange
            var user = new User 
            { 
                UserId = 6, 
                Username = "updateuser",
                FirstName = "Test",
                LastName = "User",
                PasswordHash = "hash",
                PasswordSalt = "salt"
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            user.Username = "updated";
            await _repository.UpdateUserAsync(user);

            // Assert
            var updatedUser = await _context.Users.FindAsync(6);
            Assert.NotNull(updatedUser);
            Assert.Equal("updated", updatedUser.Username);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldDeleteUser()
        {
            // Arrange
            var user = new User 
            { 
                UserId = 7, 
                Username = "deleteuser",
                FirstName = "Test",
                LastName = "User",
                PasswordHash = "hash",
                PasswordSalt = "salt"
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteUserAsync(7);

            // Assert
            var deletedUser = await _context.Users.FindAsync(7);
            Assert.Null(deletedUser);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldNotThrow_WhenUserDoesNotExist()
        {
            // Act & Assert
            await _repository.DeleteUserAsync(999); // Should not throw
        }
    }
} 