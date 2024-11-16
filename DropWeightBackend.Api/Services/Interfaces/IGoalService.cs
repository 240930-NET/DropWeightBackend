using DropWeightBackend.Api.DTOs;
using DropWeightBackend.Domain.Entities;

namespace DropWeightBackend.Api.Services.Interfaces
{
    public interface IGoalService {

        public Task<List<Goal>> GetAllGoals();

        public Task<Goal?> GetGoalById(int id);

        public Task<Goal> AddGoal(GoalDto goalDTO);

        public Task<Goal> UpdateGoal(GoalDto goalDTO, int id);

        public Task DeleteGoal(int id);

    }
}