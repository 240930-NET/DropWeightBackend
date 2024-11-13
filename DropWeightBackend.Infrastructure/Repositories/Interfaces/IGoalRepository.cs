using DropWeight.Domain.Entities;

namespace DropWeight.Infrastructure.Repositories.Interfaces;

public interface IGoalRepository {

    public Task<List<Goal>> GetAllGoals();

    public Task<Goal> GetGoalById(int id);

    public Task<Goal> AddGoal(Goal goal);

    public Task<Goal> UpdateGoal(Goal goal);

    public Task DeleteGoal(Goal goal);

}