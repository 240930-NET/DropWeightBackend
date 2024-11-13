using System.Collections.Generic;
using System.Threading.Tasks;
using DropWeight.Domain.Entities;

namespace DropWeight.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByUserIdAsync(int userId);
        Task<IEnumerable<User>> GetAllUserAsync();
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int userId);
    }
}
 