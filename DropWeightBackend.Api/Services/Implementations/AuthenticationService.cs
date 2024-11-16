using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DropWeightBackend.Domain.Entities;
using DropWeightBackend.Infrastructure.Repositories.Interfaces;
using Microsoft.IdentityModel.Tokens;
using DropWeightBackend.Api.Services.Interfaces;

namespace DropWeightBackend.Api.Services.Implementations

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly string _jwtSecretKey;

        public AuthenticationService(IUserRepository userRepository, string jwtSecretKey)
        {
            _userRepository = userRepository;
            _jwtSecretKey = jwtSecretKey;
        }

        public async Task<string> AuthenticateUserAsync(string username, string password)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user != null && VerifyPassword(user, password))
            {
                return GenerateJwtToken(user);
            }
            return null; 
        }

        public async Task<bool> RegisterUserAsync(User user, string password)
        {
            if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(password) 
                || string.IsNullOrWhiteSpace(user.FirstName) || string.IsNullOrWhiteSpace(user.LastName))
            {
                throw new ArgumentException("Username, password, first name, and last name cannot be empty.");
            }
            user.WorkoutSchedules ??= new List<WorkoutSchedule>();
            user.PasswordSalt = GenerateSalt();
            user.PasswordHash = HashPassword(password, user.PasswordSalt);
            await _userRepository.AddUserAsync(user);
            return true;
        }

        public bool VerifyPassword(User user, string password)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(Convert.FromBase64String(user.PasswordSalt)))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(computedHash) == user.PasswordHash;
            }
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSecretKey); 
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddHours(1), // Token expiration set to 1 hour
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private static string GenerateSalt()
        {
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                var salt = new byte[16];
                rng.GetBytes(salt);
                return Convert.ToBase64String(salt);
            }
        }

        private static string HashPassword(string password, string salt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(Convert.FromBase64String(salt)))
            {
                var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(passwordHash);
            }
        }
    }
}
