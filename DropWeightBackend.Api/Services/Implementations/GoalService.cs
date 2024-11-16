using DropWeightBackend.Domain.Entities;
using DropWeightBackend.Infrastructure.Repositories.Interfaces;
using DropWeightBackend.Api.Services.Interfaces;
using DropWeightBackend.Domain.Enums;
using DropWeightBackend.Api.DTOs;

namespace DropWeightBackend.Api.Services.Implementations {
    public class GoalService : IGoalService {

        public readonly IGoalRepository _goalRepo;

        public GoalService(IGoalRepository goalRepo) {
            _goalRepo = goalRepo;
        }



        public double CalculateProgress(double startingValue, double currentValue, double targetValue)
        {
            if (targetValue == startingValue)
            {
                throw new ArgumentException("Starting value and target value cannot be the same.");
            }
            
            return ((currentValue - startingValue) / (targetValue - startingValue)) * 100;
        }



        public async Task<List<Goal>> GetAllGoals() {
            return await _goalRepo.GetAllGoals();
        }



        public async Task<Goal?> GetGoalById(int id) {
            return await _goalRepo.GetGoalById(id);        
        }



        public async Task<Goal> AddGoal(GoalDto goalDTO) {

            Goal goal = new Goal();

            if (goalDTO.Type != GoalType.Custom) {              // for non Custom goal types
                if (!goalDTO.StartingValue.HasValue || !goalDTO.CurrentValue.HasValue || !goalDTO.TargetValue.HasValue) {
                    throw new Exception("Must enter a number for Starting, Current, and Target values");
                }
                else if (goalDTO.StartingValue <= 0 || goalDTO.CurrentValue <= 0 || goalDTO.TargetValue <=0) {
                    throw new Exception("Starting, Current, and Target values must be positive");
                }
                else {
                    double progress = CalculateProgress(goalDTO.StartingValue!.Value, goalDTO.CurrentValue!.Value, goalDTO.TargetValue!.Value);
                    goal.Progress = progress;               //Calculate and set progress based on user input

                    if (progress >= 100) {                  //Determine if goal is complete or not based on progress
                        goal.IsAchieved = true;
                    }
                    else {
                        goal.IsAchieved = false;
                    }
                }
            }

            else {                        //for Custom goal types
                if (string.IsNullOrEmpty(goal.Description) || string.IsNullOrEmpty(goal.GoalName) ) {
                    throw new Exception("Must enter either a description or name for a custom goal type");

                }
                else if (!goal.IsAchieved.HasValue) {
                    throw new Exception("Must mark goal as achieved or in progress for custom goal type");
                }
                else {
                    goal.IsAchieved = goalDTO.IsAchieved;
                }
            }

            goal.Type = goalDTO.Type;
            goal.GoalName = goalDTO.GoalName;
            goal.Description = goalDTO.Description;
            goal.StartingValue = goalDTO.StartingValue;
            goal.TargetValue = goalDTO.TargetValue;
            goal.CurrentValue = goalDTO.CurrentValue;

            return await _goalRepo.AddGoal(goal);
        }



        public async Task<Goal> UpdateGoal(GoalDto goalDTO, int id) {
            Goal searchedGoal = await _goalRepo.GetGoalById(id);
            if (searchedGoal == null) {
                throw new Exception($"No goal with id: {id}");
            }

            if (goalDTO.Type != GoalType.Custom) {              // for non Custom goal types
                if (!goalDTO.StartingValue.HasValue || !goalDTO.CurrentValue.HasValue || !goalDTO.TargetValue.HasValue) {
                    throw new Exception("Must enter a number for Starting, Current, and Target values");
                }
                else if (goalDTO.StartingValue <= 0 || goalDTO.CurrentValue <= 0 || goalDTO.TargetValue <=0) {
                    throw new Exception("Starting, Current, and Target values must be positive");
                }
                else {
                    double progress = CalculateProgress(goalDTO.StartingValue!.Value, goalDTO.CurrentValue!.Value, goalDTO.TargetValue!.Value);
                    searchedGoal.Progress = progress;               //Recalculate and set progress based on updated user input

                    if (progress >= 100) {                  //Determine if goal is complete or not based on updated progress
                        searchedGoal.IsAchieved = true;
                    }
                    else {
                        searchedGoal.IsAchieved = false;
                    }
                }
            }

            else {                        //for Custom goal types
                if (string.IsNullOrEmpty(goalDTO.Description) || string.IsNullOrEmpty(goalDTO.GoalName) ) {
                    throw new Exception("Must enter either a description or name for a custom goal type");

                }
                else if (!goalDTO.IsAchieved.HasValue) {
                    throw new Exception("Must mark goal as achieved or in progress for custom goal type");
                }
                else {
                    searchedGoal.IsAchieved = goalDTO.IsAchieved;    //Update whether goal is achieved or not for custom goal type
                }
            }

            searchedGoal.Type = goalDTO.Type;
            searchedGoal.CurrentValue = goalDTO.CurrentValue;
            searchedGoal.StartingValue = goalDTO.StartingValue;
            searchedGoal.TargetValue = goalDTO.TargetValue;
            searchedGoal.Description = goalDTO.Description;
            searchedGoal.GoalName = goalDTO.GoalName;

            return await _goalRepo.UpdateGoal(searchedGoal);   
        }



        public async Task DeleteGoal(int id) {
            Goal searchedGoal = await _goalRepo.GetGoalById(id);
            if (searchedGoal == null) {
                throw new Exception($"No goal with id: {id}");
            }
            else {
                await _goalRepo.DeleteGoal(searchedGoal);
            }
        }
        


    }
}