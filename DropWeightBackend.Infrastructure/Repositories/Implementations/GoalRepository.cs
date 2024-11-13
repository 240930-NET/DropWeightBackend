using DropWeightBackend.Domain.Entities;
using DropWeightBackend.Infrastructure.Data;
using DropWeightBackend.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DropWeightBackend.Infrastructure.Repositories.Implementations;

public class GoalRepository : IGoalRepository{

    private readonly DropWeightContext _context;

    public GoalRepository(DropWeightContext context) {
        _context = context;
    }


    public async Task<List<Goal>> GetAllGoals() {
        return await _context.Goals.ToListAsync();
    }

    public async Task<Goal> GetGoalById(int id) {
        return await _context.Goals.FindAsync(id);
    }
    
    public async Task<Goal> AddGoal(Goal goal) {
        await _context.Goals.AddAsync(goal);
        await _context.SaveChangesAsync();
        return goal;
    }

    public async Task<Goal> UpdateGoal(Goal goal) {
        _context.Goals.Update(goal);
        await _context.SaveChangesAsync();
        return goal;
    }

    public async Task DeleteGoal(Goal goal) {
        _context.Goals.Remove(goal);
        await _context.SaveChangesAsync();
    }

}
