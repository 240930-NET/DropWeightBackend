using Xunit;
using Moq;
using DropWeightBackend.Api.Services.Implementations;
using DropWeightBackend.Domain.Entities;
using DropWeightBackend.Infrastructure.UnitOfWork;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace DropWeightBackend.Api.DTOs;
using System;



   public class GoalServiceTests
{  
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly GoalService _goalService;

    public GoalServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _goalService = new GoalService(_mockUnitOfWork.Object);
    }

    [Fact]
public void CalculateProgress_ShouldThrowArgumentException_WhenStartingValueEqualsTargetValue()
{
    double startingValue = 100;
    double currentValue = 50;
    double targetValue = 100;

    Assert.Throws<ArgumentException>(() => _goalService.CalculateProgress(startingValue, currentValue, targetValue));
}

[Fact]
public void CalculateProgress_ShouldReturnCorrectPercentage()
{
    double startingValue = 0;
    double currentValue = 50;
    double targetValue = 100;

    double result = _goalService.CalculateProgress(startingValue, currentValue, targetValue);

    Assert.Equal(50, result);
}

[Fact]
public async Task AddGoal_ShouldThrowException_WhenValuesAreInvalid()
{
    var goalDto = new GoalDto
    {
        StartingValue = null,
        CurrentValue = null,
        TargetValue = null
    };

    await Assert.ThrowsAsync<Exception>(async () => await _goalService.AddGoal(goalDto));
}

[Fact]
public async Task AddGoal_ShouldAddGoalSuccessfully()
{
    var goalDto = new GoalDto
    {
        StartingValue = 1,  
        CurrentValue = 10,  
        TargetValue = 100,  
        GoalName = "Test Goal"
    };

    var goal = new Goal();
    _mockUnitOfWork.Setup(uow => uow.Goals.AddGoal(It.IsAny<Goal>())).ReturnsAsync(goal);
    _mockUnitOfWork.Setup(uow => uow.CompleteAsync()).ReturnsAsync(1);

    var result = await _goalService.AddGoal(goalDto);

    Assert.NotNull(result);
    _mockUnitOfWork.Verify(uow => uow.Goals.AddGoal(It.IsAny<Goal>()), Times.Once);
    _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
}

[Fact]
public async Task UpdateGoal_ShouldThrowException_WhenGoalDoesNotExist()
{
    int nonExistentGoalId = 999;
    var goalDto = new GoalDto();

    _mockUnitOfWork.Setup(uow => uow.Goals.GetGoalById(nonExistentGoalId)).ReturnsAsync((Goal)null);

    await Assert.ThrowsAsync<Exception>(async () => await _goalService.UpdateGoal(goalDto, nonExistentGoalId));
}

[Fact]
public async Task UpdateGoal_ShouldUpdateGoalSuccessfully()
{
    int existingGoalId = 1;

    var goalDto = new GoalDto
    {
        StartingValue = 10,   
        CurrentValue = 20,    
        TargetValue = 100,    
        GoalName = "Updated Goal"
    };

    var goal = new Goal
    {
        GoalId = existingGoalId,
        StartingValue = 5,
        CurrentValue = 15,
        TargetValue = 80,
        GoalName = "Old Goal"
    };

    _mockUnitOfWork.Setup(uow => uow.Goals.GetGoalById(existingGoalId)).ReturnsAsync(goal);

    _mockUnitOfWork.Setup(uow => uow.CompleteAsync()).ReturnsAsync(1);

    var result = await _goalService.UpdateGoal(goalDto, existingGoalId);

    Assert.NotNull(result);
    Assert.Equal("Updated Goal", result.GoalName);  // Make sure the name has been updated
    _mockUnitOfWork.Verify(uow => uow.Goals.UpdateGoal(goal), Times.Once);
    _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
}

[Fact]
public async Task DeleteGoal_ShouldThrowException_WhenGoalDoesNotExist()
{
    int nonExistentGoalId = 999;

    _mockUnitOfWork.Setup(uow => uow.Goals.GetGoalById(nonExistentGoalId)).ReturnsAsync((Goal)null);

    await Assert.ThrowsAsync<Exception>(async () => await _goalService.DeleteGoal(nonExistentGoalId));
}

[Fact]
public async Task DeleteGoal_ShouldDeleteGoalSuccessfully()
{
    int existingGoalId = 1;
    var goal = new Goal { GoalId = existingGoalId };

    _mockUnitOfWork.Setup(uow => uow.Goals.GetGoalById(existingGoalId)).ReturnsAsync(goal);
    _mockUnitOfWork.Setup(uow => uow.CompleteAsync()).ReturnsAsync(1);

    await _goalService.DeleteGoal(existingGoalId);

    _mockUnitOfWork.Verify(uow => uow.Goals.DeleteGoal(goal), Times.Once);
    _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
}

}



