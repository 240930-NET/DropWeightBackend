using DropWeightBackend.Domain.Entities;
using DropWeightBackend.Infrastructure.Repositories.Interfaces;

public class GoalService : IGoalService {

    public readonly IGoalRepository _goalRepo;

    public GoalService(IGoalRepository goalRepo) {
        _goalRepo = goalRepo;
    }


    public async Task<List<Goal>> GetAllGoals() {
        return await _goalRepo.GetAllGoals();
    }

    public async Task<Goal> GetGoalById(int id) {
        return await _goalRepo.GetGoalById(id);         //handle user not found in controller
    }

    public async Task<Goal> AddGoal(Goal goal) {
        if (string.IsNullOrEmpty(goal.Description)) {
            throw new Exception("Cannot have empty description!");
        }
        return await _goalRepo.AddGoal(goal);
    }

    public async Task<Goal> UpdateGoal(Goal goal) {
        Goal searchedGoal = await _goalRepo.GetGoalById(goal.GoalId);
        if (searchedGoal == null) {
            throw new Exception($"No goal with id: {goal.GoalId}");
        }
        else if (string.IsNullOrEmpty(goal.Description)) {
            throw new Exception("Cannot have empty descripton!");
        }
        else {
            searchedGoal.Type = goal.Type;
            searchedGoal.Description = goal.Description;
            return await _goalRepo.UpdateGoal(searchedGoal);
        }
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