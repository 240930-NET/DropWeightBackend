
using DropWeightBackend.Domain.Entities;

public interface IGoalService {

    public Task<List<Goal>> GetAllGoals();

    public Task<Goal> GetGoalById(int id);

    public Task<Goal> AddGoal(Goal goal);

    public Task<Goal> UpdateGoal(Goal goal);

    public Task DeleteGoal(int id);

}