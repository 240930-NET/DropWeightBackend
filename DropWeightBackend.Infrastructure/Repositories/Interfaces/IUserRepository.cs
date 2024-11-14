using DropWeightBackend.Domain.Entities;

namespace DropWeightBackend.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(int userId);
        Task<User> GetUserByUsernameAsync(string username);
        Task<IEnumerable<User>> GetAllUserAsync();
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int userId);
    }
}
