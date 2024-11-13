using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DropWeight.Domain.Entities;
using DropWeight.Infrastructure.Data;

namespace DropWeight.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DropWeightContext _context;

        public UserRepository(DropWeightContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.Workouts)
                .Include(u => u.Goals)
                .Include(u => u.Nutritions)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<IEnumerable<User>> GetAllUserAsync()
        {
            return await _context.Users
                .Include(u => u.Workouts)
                .Include(u => u.Goals)
                .Include(u => u.Nutritions)
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
