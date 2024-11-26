using Xunit;
using Moq;
using DropWeightBackend.Api.Services.Implementations;
using DropWeightBackend.Domain.Entities;
using DropWeightBackend.Infrastructure.UnitOfWork;
using DropWeightBackend.Infrastructure.Repositories.Interfaces;
using DropWeightBackend.Domain.Enums;
using DropWeightBackend.Api.DTOs;

namespace DropWeightBackend.Tests
{
    public class GoalServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IGoalRepository> _mockGoalRepository;
        private readonly GoalService _goalService;
        private readonly User _testUser;

        public GoalServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockGoalRepository = new Mock<IGoalRepository>();
            _mockUnitOfWork.Setup(uow => uow.Goals).Returns(_mockGoalRepository.Object);
            _goalService = new GoalService(_mockUnitOfWork.Object);

            _testUser = new User
            {
                UserId = 1,
                Username = "testuser"
            };
        }

        [Fact]
        public void CalculateProgress_ShouldCalculateCorrectly()
        {
            // Arrange
            double startingValue = 100;
            double currentValue = 150;
            double targetValue = 200;

            // Act
            double progress = _goalService.CalculateProgress(startingValue, currentValue, targetValue);

            // Assert
            Assert.Equal(50, progress); // Should be 50%
        }

        [Fact]
        public void CalculateProgress_ShouldThrowException_WhenStartAndTargetAreSame()
        {
            // Arrange
            double startingValue = 100;
            double currentValue = 150;
            double targetValue = 100;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => 
                _goalService.CalculateProgress(startingValue, currentValue, targetValue));
        }

        [Fact]
        public async Task GetAllGoals_ShouldReturnAllGoals()
        {
            // Arrange
            var goals = new List<Goal>
            {
                new Goal { GoalId = 1, Type = GoalType.Weight },
                new Goal { GoalId = 2, Type = GoalType.Strength }
            };

            _mockGoalRepository.Setup(repo => repo.GetAllGoals())
                .ReturnsAsync(goals);

            // Act
            var result = await _goalService.GetAllGoals();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetGoalById_ShouldReturnGoal_WhenExists()
        {
            // Arrange
            var goal = new Goal { GoalId = 1, Type = GoalType.Weight };
            _mockGoalRepository.Setup(repo => repo.GetGoalById(1))
                .ReturnsAsync(goal);

            // Act
            var result = await _goalService.GetGoalById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.GoalId);
        }

        [Fact]
        public async Task AddGoal_ShouldSucceed_ForNonCustomGoal()
        {
            // Arrange
            var goalDto = new GoalDto
            {
                Type = GoalType.Weight,
                StartingValue = 100,
                CurrentValue = 90,
                TargetValue = 80,
                GoalName = "Weight Loss Goal"
            };

            var expectedGoal = new Goal
            {
                GoalId = 1,
                Type = GoalType.Weight,
                Progress = 50,
                IsAchieved = false
            };

            _mockGoalRepository.Setup(repo => repo.AddGoal(It.IsAny<Goal>()))
                .ReturnsAsync(expectedGoal);

            // Act
            var result = await _goalService.AddGoal(goalDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(GoalType.Weight, result.Type);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task AddGoal_ShouldSucceed_ForCustomGoal()
        {
            // Arrange
            var goalDto = new GoalDto
            {
                Type = GoalType.Custom,
                GoalName = "Custom Goal",
                Description = "Custom Goal Description",
                IsAchieved = false
            };

            var expectedGoal = new Goal
            {
                GoalId = 1,
                Type = GoalType.Custom,
                GoalName = "Custom Goal",
                Description = "Custom Goal Description",
                IsAchieved = false
            };

            _mockGoalRepository.Setup(repo => repo.AddGoal(It.IsAny<Goal>()))
                .ReturnsAsync(expectedGoal);

            // Act
            var result = await _goalService.AddGoal(goalDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(GoalType.Custom, result.Type);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task AddGoal_ShouldThrowException_WhenNonCustomGoalMissingValues()
        {
            // Arrange
            var goalDto = new GoalDto
            {
                Type = GoalType.Weight,
                StartingValue = null,
                CurrentValue = 90,
                TargetValue = 80
            };

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _goalService.AddGoal(goalDto));
        }

        [Fact]
        public async Task AddGoal_ShouldThrowException_WhenNonCustomGoalHasNegativeValues()
        {
            // Arrange
            var goalDto = new GoalDto
            {
                Type = GoalType.Weight,
                StartingValue = -100,
                CurrentValue = 90,
                TargetValue = 80
            };

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _goalService.AddGoal(goalDto));
        }

        [Fact]
        public async Task AddGoal_ShouldThrowException_WhenCustomGoalMissingNameAndDescription()
        {
            // Arrange
            var goalDto = new GoalDto
            {
                Type = GoalType.Custom,
                IsAchieved = false
            };

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _goalService.AddGoal(goalDto));
        }

        [Fact]
        public async Task UpdateGoal_ShouldSucceed_WhenGoalExists()
        {
            // Arrange
            var goalId = 1;
            var existingGoal = new Goal
            {
                GoalId = goalId,
                Type = GoalType.Weight
            };

            var goalDto = new GoalDto
            {
                Type = GoalType.Weight,
                StartingValue = 100,
                CurrentValue = 90,
                TargetValue = 80
            };

            _mockGoalRepository.Setup(repo => repo.GetGoalById(goalId))
                .ReturnsAsync(existingGoal);
            _mockGoalRepository.Setup(repo => repo.UpdateGoal(It.IsAny<Goal>()))
                .ReturnsAsync(existingGoal);

            // Act
            var result = await _goalService.UpdateGoal(goalDto, goalId);

            // Assert
            Assert.NotNull(result);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateGoal_ShouldThrowException_WhenGoalNotFound()
        {
            // Arrange
            var goalId = 999;
            var goalDto = new GoalDto
            {
                Type = GoalType.Weight,
                StartingValue = 100,
                CurrentValue = 90,
                TargetValue = 80
            };

            _mockGoalRepository.Setup(repo => repo.GetGoalById(goalId))
                .ReturnsAsync((Goal?)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _goalService.UpdateGoal(goalDto, goalId));
        }

        [Fact]
        public async Task DeleteGoal_ShouldSucceed_WhenGoalExists()
        {
            // Arrange
            var goalId = 1;
            var existingGoal = new Goal { GoalId = goalId };

            _mockGoalRepository.Setup(repo => repo.GetGoalById(goalId))
                .ReturnsAsync(existingGoal);

            // Act
            await _goalService.DeleteGoal(goalId);

            // Assert
            _mockGoalRepository.Verify(repo => repo.DeleteGoal(existingGoal), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteGoal_ShouldThrowException_WhenGoalNotFound()
        {
            // Arrange
            var goalId = 999;
            _mockGoalRepository.Setup(repo => repo.GetGoalById(goalId))
                .ReturnsAsync((Goal?)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _goalService.DeleteGoal(goalId));
        }

        [Fact]
        public async Task AddGoal_ShouldThrowException_WhenCustomGoalMissingIsAchieved()
        {
            // Arrange
            var goalDto = new GoalDto
            {
                Type = GoalType.Custom,
                GoalName = "Custom Goal",
                Description = "Description",
                IsAchieved = null // Missing IsAchieved value
            };

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _goalService.AddGoal(goalDto));
        }

        [Fact]
        public async Task UpdateGoal_ShouldThrowException_WhenNonCustomGoalMissingValues()
        {
            // Arrange
            var goalId = 1;
            var existingGoal = new Goal
            {
                GoalId = goalId,
                Type = GoalType.Weight
            };

            var goalDto = new GoalDto
            {
                Type = GoalType.Weight,
                StartingValue = null, // Missing required value
                CurrentValue = 90,
                TargetValue = 80
            };

            _mockGoalRepository.Setup(repo => repo.GetGoalById(goalId))
                .ReturnsAsync(existingGoal);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _goalService.UpdateGoal(goalDto, goalId));
        }

        [Fact]
        public async Task UpdateGoal_ShouldThrowException_WhenNonCustomGoalHasNegativeValues()
        {
            // Arrange
            var goalId = 1;
            var existingGoal = new Goal
            {
                GoalId = goalId,
                Type = GoalType.Weight
            };

            var goalDto = new GoalDto
            {
                Type = GoalType.Weight,
                StartingValue = -100, // Negative value
                CurrentValue = 90,
                TargetValue = 80
            };

            _mockGoalRepository.Setup(repo => repo.GetGoalById(goalId))
                .ReturnsAsync(existingGoal);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _goalService.UpdateGoal(goalDto, goalId));
        }

        [Fact]
        public async Task UpdateGoal_ShouldThrowException_WhenCustomGoalMissingNameAndDescription()
        {
            // Arrange
            var goalId = 1;
            var existingGoal = new Goal
            {
                GoalId = goalId,
                Type = GoalType.Custom
            };

            var goalDto = new GoalDto
            {
                Type = GoalType.Custom,
                IsAchieved = false,
                GoalName = "", // Empty name
                Description = "" // Empty description
            };

            _mockGoalRepository.Setup(repo => repo.GetGoalById(goalId))
                .ReturnsAsync(existingGoal);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _goalService.UpdateGoal(goalDto, goalId));
        }

        [Fact]
        public async Task UpdateGoal_ShouldThrowException_WhenCustomGoalMissingIsAchieved()
        {
            // Arrange
            var goalId = 1;
            var existingGoal = new Goal
            {
                GoalId = goalId,
                Type = GoalType.Custom
            };

            var goalDto = new GoalDto
            {
                Type = GoalType.Custom,
                GoalName = "Custom Goal",
                Description = "Description",
                IsAchieved = null // Missing IsAchieved value
            };

            _mockGoalRepository.Setup(repo => repo.GetGoalById(goalId))
                .ReturnsAsync(existingGoal);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _goalService.UpdateGoal(goalDto, goalId));
        }

        [Fact]
        public async Task UpdateGoal_ShouldSetIsAchieved_WhenProgressReaches100()
        {
            // Arrange
            var goalId = 1;
            var existingGoal = new Goal
            {
                GoalId = goalId,
                Type = GoalType.Weight
            };

            var goalDto = new GoalDto
            {
                Type = GoalType.Weight,
                StartingValue = 100,
                CurrentValue = 80,
                TargetValue = 80 // Current equals target, should set IsAchieved to true
            };

            _mockGoalRepository.Setup(repo => repo.GetGoalById(goalId))
                .ReturnsAsync(existingGoal);
            _mockGoalRepository.Setup(repo => repo.UpdateGoal(It.IsAny<Goal>()))
                .ReturnsAsync(existingGoal);

            // Act
            var result = await _goalService.UpdateGoal(goalDto, goalId);

            // Assert
            Assert.True(result.IsAchieved);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateGoal_ShouldUpdateIsAchieved_ForCustomGoal()
        {
            // Arrange
            var goalId = 1;
            var existingGoal = new Goal
            {
                GoalId = goalId,
                Type = GoalType.Custom,
                GoalName = "Old Name",
                Description = "Old Description",
                IsAchieved = false
            };

            var goalDto = new GoalDto
            {
                Type = GoalType.Custom,
                GoalName = "New Custom Goal",
                Description = "New Description",
                IsAchieved = true  // Changing IsAchieved value
            };

            _mockGoalRepository.Setup(repo => repo.GetGoalById(goalId))
                .ReturnsAsync(existingGoal);
            _mockGoalRepository.Setup(repo => repo.UpdateGoal(It.IsAny<Goal>()))
                .ReturnsAsync((Goal g) => g);

            // Act
            var result = await _goalService.UpdateGoal(goalDto, goalId);

            // Assert
            Assert.True(result.IsAchieved);
            Assert.Equal(goalDto.GoalName, result.GoalName);
            Assert.Equal(goalDto.Description, result.Description);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
            _mockGoalRepository.Verify(repo => repo.UpdateGoal(It.Is<Goal>(g => 
                g.IsAchieved == true && 
                g.GoalName == goalDto.GoalName && 
                g.Description == goalDto.Description)), 
                Times.Once);
        }
    }
} 