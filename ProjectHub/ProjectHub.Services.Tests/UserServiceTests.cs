using Microsoft.EntityFrameworkCore;

using ProjectHub.Data;
using ProjectHub.Services.Data;
using ProjectHub.Services.Data.Interfaces;
using ProjectHub.Data.Models;

namespace ProjectHub.Services.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        private ProjectHubDbContext dbContext;
        private IUserService _userService;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ProjectHubDbContext>()
                .UseInMemoryDatabase("UserServiceTestDb")
                .Options;

            dbContext = new ProjectHubDbContext(options);

            dbContext.Users.AddRange(
                new ApplicationUser { Id = Guid.NewGuid(), Email = "user1@example.com", FullName = "User One" },
                new ApplicationUser { Id = Guid.NewGuid(), Email = "user2@example.com", FullName = "User Two" }
            );
            dbContext.SaveChanges();

            _userService = new UserService(dbContext);
        }

        [Test]
        public async System.Threading.Tasks.Task GetFullNameByEmailAsync_ValidEmail_ShouldReturnFullName()
        {
            // Arrange
            string email = "user1@example.com";

            // Act
            string fullName = await _userService.GetFullNameByEmailAsync(email);

            // Assert
            Assert.That(fullName, Is.EqualTo("User One"));
        }

        [Test]
        public async System.Threading.Tasks.Task GetFullNameByEmailAsync_InvalidEmail_ShouldReturnUserNotFound()
        {
            // Arrange
            string email = "nonexistent@example.com";

            // Act
            string fullName = await _userService.GetFullNameByEmailAsync(email);

            // Assert
            Assert.That(fullName, Is.EqualTo("User not found"));
        }

        [Test]
        public async System.Threading.Tasks.Task GetUserByIdAsync_ValidId_ShouldReturnUser()
        {
            // Arrange
            string userId = dbContext.Users.First().Id.ToString();

            // Act
            ApplicationUser user = await _userService.GetUserByIdAsync(userId);

            // Assert
            Assert.IsNotNull(user);
            Assert.That(user.Email, Is.EqualTo("user1@example.com"));
        }

        [Test]
        public async System.Threading.Tasks.Task GetUserByIdAsync_InvalidId_ShouldReturnNull()
        {
            // Arrange
            string invalidId = Guid.NewGuid().ToString();

            // Act
            ApplicationUser user = await _userService.GetUserByIdAsync(invalidId);

            // Assert
            Assert.IsNull(user);
        }

        [Test]
        public async System.Threading.Tasks.Task GetAllUsersAsync_ShouldReturnAllUsers()
        {
            // Act
            List<ApplicationUser> users = await _userService.GetAllUsersAsync();

            // Assert
            Assert.IsNotNull(users);
            Assert.That(users.Count, Is.EqualTo(2));
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();
        }
    }
}