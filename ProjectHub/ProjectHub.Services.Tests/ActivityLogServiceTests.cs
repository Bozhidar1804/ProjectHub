using Microsoft.EntityFrameworkCore;

using ProjectHub.Data;
using ProjectHub.Data.Models;
using ProjectHub.Services.Data.Interfaces;
using ProjectHub.Services.Data;
using ProjectHub.Data.Models.Enums;

namespace ProjectHub.Services.Tests
{
    [TestFixture]
    public class ActivityLogServiceTests
    {
        private IActivityLogService activityLogService;
        private ProjectHubDbContext dbContext;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ProjectHubDbContext>()
                .UseInMemoryDatabase("ActivityLogServiceTestDb")
                .Options;

            dbContext = new ProjectHubDbContext(options);
            activityLogService = new ActivityLogService(dbContext);

            // Seed data
            ApplicationUser user = new ApplicationUser { Id = Guid.NewGuid(), UserName = "user1", Email = "user1@example.com", FullName = "User1" };
            Project project = new Project
            {
                Id = Guid.NewGuid(),
                Name = "Test Project",
                Description = "Test Description",
                CreatorId = Guid.NewGuid(),
                Status = ProjectStatus.InProgress,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(10),
                MaxMilestones = 3,
                IsDeleted = false
            };
            var task = new ProjectHub.Data.Models.Task { Id = Guid.NewGuid(), Title = "Test Task", ProjectId = project.Id, Description = "Task Descr" };

            dbContext.Users.Add(user);
            dbContext.Projects.Add(project);
            dbContext.Tasks.Add(task);
            dbContext.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();
        }

        [Test]
        public async System.Threading.Tasks.Task LogActionAsync_WithValidData_ShouldLogAction()
        {
            // Arrange
            var task = dbContext.Tasks.First();
            ApplicationUser user = dbContext.Users.First();

            // Act
            await activityLogService.LogActionAsync(TaskAction.Completed, task.Id.ToString(), user.Id.ToString());

            // Assert
            Assert.That(dbContext.ActivityLogs.Count(), Is.EqualTo(1));
            ActivityLog log = dbContext.ActivityLogs.First();
            Assert.That(log.Action, Is.EqualTo(TaskAction.Completed));
            Assert.That(log.TaskId, Is.EqualTo(task.Id));
            Assert.That(log.UserId, Is.EqualTo(user.Id));
        }

    }
}
