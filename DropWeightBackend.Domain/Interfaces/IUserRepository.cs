using System.Collections.Generic;
using System.Threading.Tasks;
using DropWeight.Domain.Entities;

namespace DropWeight.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int userId);
        Task<IEnumerable<User>> GetAllAsync();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int userId);
    }
}
