using System.Collections.Generic;
using System.Threading.Tasks;
using DropWeight.Domain.Entities;

namespace DropWeight.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(int userId);
        Task<IEnumerable<User>> GetAllUserAsync();
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int userId);
    }
}
