using DropWeightBackend.Domain.Entities;
using DropWeightBackend.Infrastructure.UnitOfWork;
using DropWeightBackend.Api.Services.Interfaces;
using DropWeightBackend.Domain.Enums;
using DropWeightBackend.Api.DTOs;

namespace DropWeightBackend.Api.Services.Implementations
{
    public class GoalService : IGoalService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GoalService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public double CalculateProgress(double startingValue, double currentValue, double targetValue)
        {
            if (targetValue == startingValue)
            {
                throw new ArgumentException("Starting value and target value cannot be the same.");
            }

            return ((currentValue - startingValue) / (targetValue - startingValue)) * 100;
        }

        public async Task<List<Goal>> GetAllGoals()
        {
            return await _unitOfWork.Goals.GetAllGoals();
        }

        public async Task<Goal?> GetGoalById(int id)
        {
            return await _unitOfWork.Goals.GetGoalById(id);
        }

        public async Task<Goal> AddGoal(GoalDto goalDTO)
        {
            Goal goal = new Goal();

            if (goalDTO.Type != GoalType.Custom)
            {
                if (!goalDTO.StartingValue.HasValue || !goalDTO.CurrentValue.HasValue || !goalDTO.TargetValue.HasValue)
                {
                    throw new Exception("Must enter a number for Starting, Current, and Target values");
                }
                else if (goalDTO.StartingValue <= 0 || goalDTO.CurrentValue <= 0 || goalDTO.TargetValue <= 0)
                {
                    throw new Exception("Starting, Current, and Target values must be positive");
                }
                else
                {
                    double progress = CalculateProgress(goalDTO.StartingValue!.Value, goalDTO.CurrentValue!.Value, goalDTO.TargetValue!.Value);
                    goal.Progress = progress;

                    goal.IsAchieved = progress >= 100;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(goalDTO.Description) && string.IsNullOrEmpty(goalDTO.GoalName))
                {
                    throw new Exception("Must enter either a description or name for a custom goal type");
                }
                else if (!goalDTO.IsAchieved.HasValue)
                {
                    throw new Exception("Must mark goal as achieved or in progress for custom goal type");
                }
                else
                {
                    goal.IsAchieved = goalDTO.IsAchieved;
                }
            }

            goal.Type = goalDTO.Type;
            goal.GoalName = goalDTO.GoalName;
            goal.Description = goalDTO.Description;
            goal.StartingValue = goalDTO.StartingValue;
            goal.TargetValue = goalDTO.TargetValue;
            goal.CurrentValue = goalDTO.CurrentValue;

            var addedGoal = await _unitOfWork.Goals.AddGoal(goal);
            await _unitOfWork.CompleteAsync(); // Save changes
            return addedGoal;
        }

        public async Task<Goal> UpdateGoal(GoalDto goalDTO, int id)
        {
            Goal searchedGoal = await _unitOfWork.Goals.GetGoalById(id);
            if (searchedGoal == null)
            {
                throw new Exception($"No goal with id: {id}");
            }

            if (goalDTO.Type != GoalType.Custom)
            {
                if (!goalDTO.StartingValue.HasValue || !goalDTO.CurrentValue.HasValue || !goalDTO.TargetValue.HasValue)
                {
                    throw new Exception("Must enter a number for Starting, Current, and Target values");
                }
                else if (goalDTO.StartingValue <= 0 || goalDTO.CurrentValue <= 0 || goalDTO.TargetValue <= 0)
                {
                    throw new Exception("Starting, Current, and Target values must be positive");
                }
                else
                {
                    double progress = CalculateProgress(goalDTO.StartingValue!.Value, goalDTO.CurrentValue!.Value, goalDTO.TargetValue!.Value);
                    searchedGoal.Progress = progress;

                    searchedGoal.IsAchieved = progress >= 100;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(goalDTO.Description) && string.IsNullOrEmpty(goalDTO.GoalName))
                {
                    throw new Exception("Must enter either a description or name for a custom goal type");
                }
                else if (!goalDTO.IsAchieved.HasValue)
                {
                    throw new Exception("Must mark goal as achieved or in progress for custom goal type");
                }
                else
                {
                    searchedGoal.IsAchieved = goalDTO.IsAchieved;
                }
            }

            searchedGoal.Type = goalDTO.Type;
            searchedGoal.CurrentValue = goalDTO.CurrentValue;
            searchedGoal.StartingValue = goalDTO.StartingValue;
            searchedGoal.TargetValue = goalDTO.TargetValue;
            searchedGoal.Description = goalDTO.Description;
            searchedGoal.GoalName = goalDTO.GoalName;

            var updatedGoal = await _unitOfWork.Goals.UpdateGoal(searchedGoal);
            await _unitOfWork.CompleteAsync(); // Save changes
            return updatedGoal;
        }

        public async Task DeleteGoal(int id)
        {
            Goal searchedGoal = await _unitOfWork.Goals.GetGoalById(id);
            if (searchedGoal == null)
            {
                throw new Exception($"No goal with id: {id}");
            }
            else
            {
                await _unitOfWork.Goals.DeleteGoal(searchedGoal);
                await _unitOfWork.CompleteAsync(); // Save changes
            }
        }
    }
}
