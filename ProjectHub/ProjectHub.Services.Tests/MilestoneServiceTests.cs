using Microsoft.EntityFrameworkCore;

using ProjectHub.Data;
using ProjectHub.Data.Models;
using ProjectHub.Services.Data.Interfaces;
using ProjectHub.Services.Data;
using ProjectHub.Data.Models.Enums;
using ProjectHub.Web.ViewModels.Milestone;
using static ProjectHub.Common.GeneralApplicationConstants;

namespace ProjectHub.Services.Tests
{
    [TestFixture]
    public class MilestoneServiceTests
    {
        private IMilestoneService milestoneService;
        private ProjectHubDbContext dbContext;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ProjectHubDbContext>()
                .UseInMemoryDatabase("MilestoneServiceTestDb")
                .Options;

            dbContext = new ProjectHubDbContext(options);
            milestoneService = new MilestoneService(dbContext);

            // Seed data
            Project project = new Project
            {
                Id = Guid.NewGuid(),
                Name = "Test Project",
                Description = "Test Description",
                CreatorId = Guid.NewGuid(),
                Status = ProjectStatus.InProgress,
                StartDate = new DateTime(2024, 1, 1),
                EndDate = new DateTime(2024, 12, 31),
                MaxMilestones = 3
            };
            dbContext.Projects.Add(project);
            dbContext.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();
        }

        [Test]
        public async System.Threading.Tasks.Task GetMilestonesByProjectIdAsync_WithValidProjectId_ShouldReturnMilestones()
        {
            // Arrange
            Project project = dbContext.Projects.First();
            Milestone milestone = new Milestone { Name = "Milestone 1", Deadline = project.StartDate.AddDays(10), ProjectId = project.Id };
            dbContext.Milestones.Add(milestone);
            await dbContext.SaveChangesAsync();

            // Act
            List<Milestone> result = await milestoneService.GetMilestonesByProjectIdAsync(project.Id.ToString());

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First().Id, Is.EqualTo(milestone.Id));
        }

        [Test]
        public async System.Threading.Tasks.Task CreateMilestoneAsync_WithValidData_ShouldCreateMilestone()
        {
            // Arrange
            Project project = dbContext.Projects.First();
            MilestoneCreateInputModel model = new MilestoneCreateInputModel
            {
                Name = "Milestone 1",
                Deadline = project.StartDate.AddDays(10).ToString(DateFormat),
                ProjectId = project.Id.ToString()
            };

            // Act
            bool result = await milestoneService.CreateMilestoneAsync(model);

            // Assert
            Assert.IsTrue(result);
            Assert.That(dbContext.Milestones.Count(), Is.EqualTo(1));
            Milestone milestone = dbContext.Milestones.First();
            Assert.That(milestone.Name, Is.EqualTo(model.Name));
        }

        [Test]
        public async System.Threading.Tasks.Task CreateMilestoneAsync_WithInvalidDeadline_ShouldReturnFalse()
        {
            // Arrange
            Project project = dbContext.Projects.First();
            MilestoneCreateInputModel model = new MilestoneCreateInputModel
            {
                Name = "Invalid Milestone",
                Deadline = "invalid-date",
                ProjectId = project.Id.ToString()
            };

            // Act
            bool result = await milestoneService.CreateMilestoneAsync(model);

            // Assert
            Assert.IsFalse(result);
            Assert.That(dbContext.Milestones.Count(), Is.EqualTo(0));
        }

    }
}
