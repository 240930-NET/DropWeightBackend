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
        public async Task AddUserAsync_ShouldCallRepositoryAndComplete()
        {
            // Arrange
            var user = new User { Username = "newuser" };

            // Act
            await _userService.AddUserAsync(user);

            // Assert
            _mockUserRepository.Verify(repo => repo.AddUserAsync(user), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldCallRepositoryAndComplete()
        {
            // Arrange
            var user = new User { UserId = 1, Username = "updateduser" };

            // Act
            await _userService.UpdateUserAsync(user);

            // Assert
            _mockUserRepository.Verify(repo => repo.UpdateUserAsync(user), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldCallRepositoryAndComplete()
        {
            // Arrange
            int userId = 1;

            // Act
            await _userService.DeleteUserAsync(userId);

            // Assert
            _mockUserRepository.Verify(repo => repo.DeleteUserAsync(userId), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }
    }
}