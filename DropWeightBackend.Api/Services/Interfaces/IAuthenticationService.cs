using DropWeightBackend.Domain.Entities;

namespace DropWeightBackend.Api.Services.Interfaces 
{
    public interface IAuthenticationService
    {
        Task<string> AuthenticateUserAsync(string username, string password);
        Task<bool> RegisterUserAsync(User user, string password);
        bool VerifyPassword(User user, string password);
    }
}