using System.Collections.Generic;
using System.Threading.Tasks;
using DropWeight.Domain.Entities;

namespace DropWeightBackend.Services 
{
    public interface IAuthenticationService
    {
        Task<string> AuthenticateUserAsync(string username, string password);
        Task<bool> RegisterUserAsync(User user, string password);
        bool VerifyPassword(User user, string password);
    }
}