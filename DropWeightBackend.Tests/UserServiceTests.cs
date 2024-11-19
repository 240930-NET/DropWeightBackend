using Moq;
using Xunit;
using DropWeightBackend.Api.Services.Implementations;
using DropWeightBackend.Domain.Entities;
using DropWeightBackend.Infrastructure.UnitOfWork;
using DropWeightBackend.Infrastructure.Repositories.Interfaces;

namespace DropWeightBackend.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockUnitOfWork.Setup(uow => uow.Users).Returns(_mockUserRepository.Object);
            _userService = new UserService(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var expectedUser = new User { UserId = 1, Username = "testuser" };
            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(1))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await _userService.GetUserByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUser.UserId, result.UserId);
            Assert.Equal(expectedUser.Username, result.Username);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(999))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _userService.GetUserByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnAllUsers()
        {
            // Arrange
            var expectedUsers = new List<User>
            {
                new User { UserId = 1, Username = "user1" },
                new User { UserId = 2, Username = "user2" }
            };
            _mockUserRepository.Setup(repo => repo.GetAllUsersAsync())
                .ReturnsAsync(expectedUsers);

            // Act
            var result = await _userService.GetAllUsersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUsers.Count, result.Count());
        }

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnEmptyList_WhenNoUsers()
        {
            // Arrange
            _mockUserRepository.Setup(repo => repo.GetAllUsersAsync())
                .ReturnsAsync(new List<User>());

            // Act
            var result = await _userService.GetAllUsersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task AddUserAsync_ShouldCallRepositoryAndComplete()
        {
            // Arrange
            var user = new User { Username = "newuser" };
            _mockUserRepository.Setup(repo => repo.AddUserAsync(It.IsAny<User>()))
                .Returns(Task.CompletedTask);

            // Act
            await _userService.AddUserAsync(user);

            // Assert
            _mockUserRepository.Verify(repo => repo.AddUserAsync(user), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task AddUserAsync_ShouldThrowException_WhenUserIsNull()
        {
            // Arrange
            User? nullUser = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => 
                _userService.AddUserAsync(nullUser!));
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldCallRepositoryAndComplete()
        {
            // Arrange
            var user = new User { UserId = 1, Username = "updateduser" };
            _mockUserRepository.Setup(repo => repo.UpdateUserAsync(It.IsAny<User>()))
                .Returns(Task.CompletedTask);

            // Act
            await _userService.UpdateUserAsync(user);

            // Assert
            _mockUserRepository.Verify(repo => repo.UpdateUserAsync(user), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldThrowException_WhenUserIsNull()
        {
            // Arrange
            User? nullUser = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => 
                _userService.UpdateUserAsync(nullUser!));
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldCallRepositoryAndComplete()
        {
            // Arrange
            int userId = 1;
            _mockUserRepository.Setup(repo => repo.DeleteUserAsync(It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            // Act
            await _userService.DeleteUserAsync(userId);

            // Assert
            _mockUserRepository.Verify(repo => repo.DeleteUserAsync(userId), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldThrowException_WhenUserIdIsNegative()
        {
            // Arrange
            int invalidUserId = -1;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _userService.DeleteUserAsync(invalidUserId));
        }

        [Fact]
        public void VerifyPassword_ShouldVerifyCorrectly()
        {
            // Arrange
            var user = new User
            {
                UserId = 1,
                Username = "testuser",
                PasswordSalt = Convert.ToBase64String(new byte[] { 1, 2, 3, 4, 5 }), // Sample salt
                PasswordHash = "" // We'll set this after computing with the same salt
            };

            // Compute the actual hash that would match
            var password = "testpassword";
            using (var hmac = new System.Security.Cryptography.HMACSHA512(Convert.FromBase64String(user.PasswordSalt)))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                user.PasswordHash = Convert.ToBase64String(computedHash);
            }

            // Use reflection to access the private method
            var methodInfo = typeof(UserService).GetMethod("VerifyPassword", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            // Act
            var result = (bool)methodInfo.Invoke(_userService, new object[] { user, password });
            var wrongResult = (bool)methodInfo.Invoke(_userService, new object[] { user, "wrongpassword" });

            // Assert
            Assert.True(result);
            Assert.False(wrongResult);
        }
    }
}