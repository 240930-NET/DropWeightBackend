using System.Collections.Generic;
using System.Threading.Tasks;
using DropWeight.Domain.Entities;

namespace DropWeight.Services
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(int userId);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int userId);
    }
}
