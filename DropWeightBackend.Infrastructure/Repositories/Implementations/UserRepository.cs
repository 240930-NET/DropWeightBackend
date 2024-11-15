using Microsoft.EntityFrameworkCore;
using DropWeightBackend.Domain.Entities;
using DropWeightBackend.Infrastructure.Data;
using DropWeightBackend.Infrastructure.Repositories.Interfaces;

namespace DropWeightBackend.Infrastructure.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly DropWeightContext _context;

        public UserRepository(DropWeightContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.Workouts)
                .Include(u => u.Goals)
                .Include(u => u.Nutritions)
                .Include(u => u.WorkoutSchedules) 
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(u => u.Workouts)
                .Include(u => u.Goals)
                .Include(u => u.Nutritions)
                .Include(u => u.WorkoutSchedules) 
                .FirstOrDefaultAsync(u => u.Username == username);  
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.Workouts)
                .Include(u => u.Goals)
                .Include(u => u.Nutritions)
                .Include(u => u.WorkoutSchedules) 
                .ToListAsync();
        }
        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await GetUserByIdAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
