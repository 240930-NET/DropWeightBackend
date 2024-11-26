using Moq;
using Xunit;
using System.Text;
using DropWeightBackend.Api.Services.Implementations;
using DropWeightBackend.Domain.Entities;
using DropWeightBackend.Infrastructure.UnitOfWork;
using DropWeightBackend.Infrastructure.Repositories.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DropWeightBackend.Tests
{
    public class AuthenticationServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly AuthenticationService _authService;
        private readonly string _jwtSecretKey;

        public AuthenticationServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockUnitOfWork.Setup(uow => uow.Users).Returns(_mockUserRepository.Object);
            _jwtSecretKey = "your-256-bit-secret-your-256-bit-secret-your-256-bit-secret"; // At least 32 characters
            _authService = new AuthenticationService(_mockUnitOfWork.Object, _jwtSecretKey);
        }

        [Fact]
        public async Task AuthenticateUserAsync_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange
            var password = "testpassword";
            var user = new User
            {
                UserId = 1,
                Username = "testuser",
                PasswordSalt = Convert.ToBase64String(new byte[] { 1, 2, 3, 4, 5 })
            };

            // Generate actual hash
            using (var hmac = new System.Security.Cryptography.HMACSHA512(Convert.FromBase64String(user.PasswordSalt)))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                user.PasswordHash = Convert.ToBase64String(computedHash);
            }

            _mockUserRepository.Setup(repo => repo.GetUserByUsernameAsync("testuser"))
                .ReturnsAsync(user);

            // Act
            var token = await _authService.AuthenticateUserAsync("testuser", password);

            // Assert
            Assert.NotNull(token);
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            // Debug: Print all claims to see what's actually in the token
            foreach (var claim in jwtToken.Claims)
            {
                System.Diagnostics.Debug.WriteLine($"Claim Type: {claim.Type}, Value: {claim.Value}");
            }

            // Use the actual claim types from System.Security.Claims
            var nameIdentifierClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid" || 
                                                         c.Type == ClaimTypes.NameIdentifier ||
                                                         c.Type == "sub");
            var nameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "unique_name" || 
                                                       c.Type == ClaimTypes.Name ||
                                                       c.Type == "name");

            Assert.NotNull(nameIdentifierClaim);
            Assert.Null(nameClaim);
            Assert.Equal(user.UserId.ToString(), nameIdentifierClaim.Value);
           
     }

        [Fact]
        public async Task AuthenticateUserAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            _mockUserRepository.Setup(repo => repo.GetUserByUsernameAsync("nonexistent"))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _authService.AuthenticateUserAsync("nonexistent", "anypassword");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AuthenticateUserAsync_ShouldReturnNull_WhenPasswordIsIncorrect()
        {
            // Arrange
            var user = new User
            {
                UserId = 1,
                Username = "testuser",
                PasswordSalt = Convert.ToBase64String(new byte[] { 1, 2, 3, 4, 5 }),
                PasswordHash = "somehash"
            };

            _mockUserRepository.Setup(repo => repo.GetUserByUsernameAsync("testuser"))
                .ReturnsAsync(user);

            // Act
            var result = await _authService.AuthenticateUserAsync("testuser", "wrongpassword");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldSucceed_WhenInputIsValid()
        {
            // Arrange
            var user = new User
            {
                Username = "newuser",
                FirstName = "John",
                LastName = "Doe"
            };

            _mockUserRepository.Setup(repo => repo.AddUserAsync(It.IsAny<User>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _authService.RegisterUserAsync(user, "password123");

            // Assert
            Assert.True(result);
            Assert.NotNull(user.PasswordHash);
            Assert.NotNull(user.PasswordSalt);
            _mockUserRepository.Verify(repo => repo.AddUserAsync(user), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Theory]
        [InlineData("", "password", "John", "Doe")]
        [InlineData("username", "", "John", "Doe")]
        [InlineData("username", "password", "", "Doe")]
        [InlineData("username", "password", "John", "")]
        [InlineData(null, "password", "John", "Doe")]
        [InlineData("username", null, "John", "Doe")]
        [InlineData("username", "password", null, "Doe")]
        [InlineData("username", "password", "John", null)]
        public async Task RegisterUserAsync_ShouldThrowException_WhenInputIsInvalid(
            string username, string password, string firstName, string lastName)
        {
            // Arrange
            var user = new User
            {
                Username = username,
                FirstName = firstName,
                LastName = lastName
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _authService.RegisterUserAsync(user, password));
        }

        [Fact]
        public void VerifyPassword_ShouldReturnTrue_WhenPasswordIsCorrect()
        {
            // Arrange
            var password = "testpassword";
            var user = new User
            {
                UserId = 1,
                Username = "testuser",
                PasswordSalt = Convert.ToBase64String(new byte[] { 1, 2, 3, 4, 5 })
            };

            using (var hmac = new System.Security.Cryptography.HMACSHA512(Convert.FromBase64String(user.PasswordSalt)))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                user.PasswordHash = Convert.ToBase64String(computedHash);
            }

            // Act
            var result = _authService.VerifyPassword(user, password);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void VerifyPassword_ShouldReturnFalse_WhenPasswordIsIncorrect()
        {
            // Arrange
            var user = new User
            {
                UserId = 1,
                Username = "testuser",
                PasswordSalt = Convert.ToBase64String(new byte[] { 1, 2, 3, 4, 5 }),
                PasswordHash = "somehash"
            };

            // Act
            var result = _authService.VerifyPassword(user, "wrongpassword");

            // Assert
            Assert.False(result);
        }
    }
} 