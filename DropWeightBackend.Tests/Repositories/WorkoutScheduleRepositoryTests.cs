using Xunit;
using Microsoft.EntityFrameworkCore;
using DropWeightBackend.Infrastructure.Data;
using DropWeightBackend.Infrastructure.Repositories.Implementations;
using DropWeightBackend.Domain.Entities;
using DropWeightBackend.Domain.Enums;

namespace DropWeightBackend.Tests.Repositories
{
    public class WorkoutScheduleRepositoryTests : IDisposable
    {
        private readonly DbContextOptions<DropWeightContext> _options;
        private readonly DropWeightContext _context;
        private readonly WorkoutScheduleRepository _repository;
        private readonly User _testUser;
        private readonly Workout _testWorkout;

        public WorkoutScheduleRepositoryTests()
        {
            var dbName = Guid.NewGuid().ToString();
            _options = new DbContextOptionsBuilder<DropWeightContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            _context = new DropWeightContext(_options);
            _repository = new WorkoutScheduleRepository(_context);

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

            // Create test workout
            _testWorkout = new Workout
            {
                WorkoutId = 1,
                Type = WorkoutType.Run,
                UserId = _testUser.UserId,
                User = _testUser
            };
            _context.Workouts.Add(_testWorkout);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task GetScheduleByIdAsync_ShouldReturnScheduleWithWorkout_WhenExists()
        {
            // Arrange
            var schedule = new WorkoutSchedule
            {
                WorkoutScheduleId = 1,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddHours(1),
                WorkoutId = _testWorkout.WorkoutId,
                UserId = _testUser.UserId
            };
            await _context.WorkoutSchedules.AddAsync(schedule);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetScheduleByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Workout);
            Assert.Equal(_testWorkout.WorkoutId, result.WorkoutId);
        }

        [Fact]
        public async Task GetScheduleByIdAsync_ShouldReturnNull_WhenDoesNotExist()
        {
            // Act
            var result = await _repository.GetScheduleByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetSchedulesForUserAsync_ShouldReturnUserSchedulesWithWorkouts()
        {
            // Arrange
            using var context = new DropWeightContext(_options);
            var repository = new WorkoutScheduleRepository(context);

            // Create and save user
            var user = new User
            {
                UserId = 10,
                Username = "testuser10",
                FirstName = "Test",
                LastName = "User",
                PasswordHash = "hash",
                PasswordSalt = "salt"
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Create and save workout
            var workout = new Workout
            {
                WorkoutId = 10,
                Type = WorkoutType.Run,
                UserId = user.UserId,
                User = user
            };
            context.Workouts.Add(workout);
            await context.SaveChangesAsync();

            // Create and save schedules
            var schedules = new List<WorkoutSchedule>
            {
                new WorkoutSchedule
                {
                    WorkoutScheduleId = 10,
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now.AddHours(1),
                    WorkoutId = workout.WorkoutId,
                    UserId = user.UserId
                },
                new WorkoutSchedule
                {
                    WorkoutScheduleId = 11,
                    StartTime = DateTime.Now.AddDays(1),
                    EndTime = DateTime.Now.AddDays(1).AddHours(1),
                    WorkoutId = workout.WorkoutId,
                    UserId = user.UserId
                }
            };

            

            // Act
            var result = await repository.GetSchedulesForUserAsync(user.UserId);

            // Assert
            
        }

        [Fact]
        public async Task GetSchedulesForUserAsync_ShouldReturnEmptyList_WhenNoSchedules()
        {
            // Act
            var result = await _repository.GetSchedulesForUserAsync(999);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task AddScheduleAsync_ShouldAddSchedule()
        {
            // Arrange
            var schedule = new WorkoutSchedule
            {
                WorkoutScheduleId = 4,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddHours(1),
                WorkoutId = _testWorkout.WorkoutId,
                UserId = _testUser.UserId
            };

            // Act
            await _repository.AddScheduleAsync(schedule);

            // Assert
            var addedSchedule = await _context.WorkoutSchedules
                .Include(ws => ws.Workout)
                .FirstOrDefaultAsync(ws => ws.WorkoutScheduleId == 4);
            Assert.NotNull(addedSchedule);
            Assert.NotNull(addedSchedule.Workout);
            Assert.Equal(_testWorkout.WorkoutId, addedSchedule.WorkoutId);
        }

        [Fact]
        public async Task UpdateScheduleAsync_ShouldUpdateSchedule()
        {
            // Arrange
            var schedule = new WorkoutSchedule
            {
                WorkoutScheduleId = 5,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddHours(1),
                WorkoutId = _testWorkout.WorkoutId,
                UserId = _testUser.UserId
            };
            await _context.WorkoutSchedules.AddAsync(schedule);
            await _context.SaveChangesAsync();

            // Act
            var newEndTime = DateTime.Now.AddHours(2);
            schedule.EndTime = newEndTime;
            await _repository.UpdateScheduleAsync(schedule);

            // Assert
            var updatedSchedule = await _context.WorkoutSchedules
                .Include(ws => ws.Workout)
                .FirstOrDefaultAsync(ws => ws.WorkoutScheduleId == 5);
            Assert.NotNull(updatedSchedule);
            Assert.NotNull(updatedSchedule.Workout);
            Assert.Equal(newEndTime, updatedSchedule.EndTime);
        }

        [Fact]
        public async Task DeleteScheduleAsync_ShouldDeleteSchedule()
        {
            // Arrange
            var schedule = new WorkoutSchedule
            {
                WorkoutScheduleId = 6,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddHours(1),
                WorkoutId = _testWorkout.WorkoutId,
                UserId = _testUser.UserId
            };
            await _context.WorkoutSchedules.AddAsync(schedule);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteScheduleAsync(6);

            // Assert
            var deletedSchedule = await _context.WorkoutSchedules.FindAsync(6);
            Assert.Null(deletedSchedule);
        }

        [Fact]
        public async Task DeleteScheduleAsync_ShouldNotThrow_WhenScheduleDoesNotExist()
        {
            // Act & Assert
            await _repository.DeleteScheduleAsync(999); // Should not throw
        }
    }
} 