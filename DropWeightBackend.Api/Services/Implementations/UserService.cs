using DropWeightBackend.Domain.Entities;
using DropWeightBackend.Infrastructure.UnitOfWork;
using DropWeightBackend.Api.Services.Interfaces;

namespace DropWeightBackend.Api.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _unitOfWork.Users.GetUserByIdAsync(userId);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _unitOfWork.Users.GetAllUsersAsync();
        }

        public async Task AddUserAsync(User user)
        {
            await _unitOfWork.Users.AddUserAsync(user);
            await _unitOfWork.CompleteAsync(); // Save changes
        }

        public async Task UpdateUserAsync(User user)
        {
            await _unitOfWork.Users.UpdateUserAsync(user);
            await _unitOfWork.CompleteAsync(); // Save changes
        }

        public async Task DeleteUserAsync(int userId)
        {
            await _unitOfWork.Users.DeleteUserAsync(userId);
            await _unitOfWork.CompleteAsync(); // Save changes
        }

        private bool VerifyPassword(User user, string password)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(Convert.FromBase64String(user.PasswordSalt)))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(computedHash) == user.PasswordHash;
            }
        }
    }
}
