using Microsoft.EntityFrameworkCore;

using ProjectHub.Data;
using ProjectHub.Data.Models;
using ProjectHub.Data.Models.Enums;
using ProjectHub.Services.Data;
using ProjectHub.Services.Data.Interfaces;
using ProjectHub.Web.ViewModels.Project;
using static ProjectHub.Common.GeneralApplicationConstants;

namespace ProjectHub.Services.Tests
{
    [TestFixture]
    public class ProjectServiceTests
    {
        private ProjectHubDbContext dbContext;
        private IProjectService projectService;
        private IActivityLogService activityLogService;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ProjectHubDbContext>()
                .UseInMemoryDatabase("ProjectServiceTestDb")
                .Options;

            this.dbContext = new ProjectHubDbContext(options);
            this.activityLogService = new ActivityLogService(this.dbContext);
            this.projectService = new ProjectService(dbContext, activityLogService);
        }
        [Test]
        public void SetMaxMilestonesAsync_ShouldThrowArgumentOutOfRangeException_WhenMaxMilestonesLessThanOrEqualToZero()
        {
            // Create and add a test project
            Project testProject = new Project
            {
                Id = Guid.NewGuid(),
                Name = "Test Project",
                Description = "Test Description",
                CreatorId = Guid.NewGuid(),
                Status = ProjectStatus.InProgress,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(10),
                MaxMilestones = null,
                IsDeleted = false
            };

            dbContext.Projects.Add(testProject);
            dbContext.SaveChanges();

            // Arrange
            int invalidMaxMilestones = 0;

            // Act & Assert
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await projectService.SetMaxMilestonesAsync(testProject.Id.ToString(), invalidMaxMilestones));
        }

        [Test]
        public async System.Threading.Tasks.Task GetCreatorAllProjectsAsync_ShouldReturnProjectsByCreatorId_WhenCalled()
        {
            // Create test users
            ApplicationUser creatorUser = new ApplicationUser { Id = Guid.NewGuid(), UserName = "creatorUser", Email = "creatorUser@example.com", FullName="CreatorUser" };

            // Create test projects
            Project testProject1 = new Project
            {
                Id = Guid.NewGuid(),
                Name = "Creator's Test Project 1",
                Description = "Test Description 1",
                CreatorId = creatorUser.Id,
                Status = ProjectStatus.InProgress,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(10),
                IsDeleted = false
            };

            Project testProject2 = new Project
            {
                Id = Guid.NewGuid(),
                Name = "Creator's Test Project 2",
                Description = "Test Description 2",
                CreatorId = creatorUser.Id,
                Status = ProjectStatus.Completed,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(20),
                IsDeleted = false
            };

            dbContext.Users.Add(creatorUser);
            dbContext.Projects.AddRange(testProject1, testProject2);
            dbContext.SaveChanges();
            // Arrange
            string creatorUserId = dbContext.Users.First().Id.ToString();

            // Act
            IEnumerable<ProjectIndexViewModel> result = await projectService.GetCreatorAllProjectsAsync(creatorUserId);

            // Assert
            Assert.That(result.Count(), Is.EqualTo(2)); // Expecting 2 projects for this creator
            Assert.IsTrue(result.Any(p => p.Name == "Creator's Test Project 1"));
            Assert.IsTrue(result.Any(p => p.Name == "Creator's Test Project 2"));
            Assert.IsFalse(result.Any(p => p.IsDeleted)); // Ensure no deleted projects are returned
        }

        [Test]
        public async System.Threading.Tasks.Task SetMaxMilestonesAsync_ShouldSetMaxMilestones_WhenProjectExistsAndValid()
        {
            // Create and add a test project
            Project testProject = new Project
            {
                Id = Guid.NewGuid(),
                Name = "Test Project",
                Description = "Test Description",
                CreatorId = Guid.NewGuid(),
                Status = ProjectStatus.InProgress,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(10),
                MaxMilestones = null,
                IsDeleted = false
            };

            await dbContext.Projects.AddAsync(testProject);
            await dbContext.SaveChangesAsync();

            // Arrange
            int newMaxMilestones = 5;

            // Act
            bool result = await projectService.SetMaxMilestonesAsync(testProject.Id.ToString(), newMaxMilestones);

            // Assert
            Assert.IsTrue(result);
            Project? updatedProject = await dbContext.Projects.FindAsync(testProject.Id);
            Assert.That(updatedProject.MaxMilestones, Is.EqualTo(newMaxMilestones));
        }

        [Test]
        public async System.Threading.Tasks.Task UpdateProjectAsync_ShouldUpdateProject_WhenValidProject()
        {
            // Create and add a test project
            Project testProject = new Project
            {
                Id = Guid.NewGuid(),
                Name = "Test Project",
                Description = "Test Description",
                CreatorId = Guid.NewGuid(),
                Status = ProjectStatus.InProgress,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(10),
                MaxMilestones = null,
                IsDeleted = false
            };

            await dbContext.Projects.AddAsync(testProject);
            await dbContext.SaveChangesAsync();

            // Arrange
            Project? projectToUpdate = await dbContext.Projects.FindAsync(testProject.Id);
            projectToUpdate.Name = "Updated Project Name";
            projectToUpdate.Description = "Updated Description";

            // Act
            await projectService.UpdateProjectAsync(projectToUpdate);
            Project? updatedProject = await dbContext.Projects.FindAsync(testProject.Id);

            // Assert
            Assert.That(updatedProject.Name, Is.EqualTo("Updated Project Name"));
            Assert.That(updatedProject.Description, Is.EqualTo("Updated Description"));
        }

        [Test]
        public async System.Threading.Tasks.Task GetAllProjectsAsync_ShouldReturnCorrectProjects_WhenCalled()
        {
            // Arrange
            Project project = new Project
            {
                Id = Guid.NewGuid(),
                Name = "Test Project",
                Description = "Test Description",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(10),
                IsDeleted = false,
                Status = 0
            };

            await this.dbContext.Projects.AddAsync(project);
            await this.dbContext.SaveChangesAsync();

            // Act
            IEnumerable<ProjectIndexViewModel> result = await this.projectService.GetAllProjectsAsync();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Name, Is.EqualTo("Test Project"));
        }

        [Test]
        public async System.Threading.Tasks.Task CreateProjectAsync_ShouldCreateProject_WhenDataIsValid()
        {
            // Arrange
            string userId = Guid.NewGuid().ToString();
            ApplicationUser user = new ApplicationUser
            {
                Id = Guid.Parse(userId),
                UserName = "TestUser",
                FullName = "Test User"
            };

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.SaveChangesAsync();

            ProjectCreateInputModel model = new ProjectCreateInputModel
            {
                Name = "New Project",
                Description = "New Project Description",
                StartDate = DateTime.UtcNow.ToString(DateFormat),
                EndDate = DateTime.UtcNow.AddDays(5).ToString(DateFormat)
            };

            // Act
            bool result = await this.projectService.CreateProjectAsync(model, userId);

            // Assert
            Assert.That(result, Is.True);
            Project? createdProject = await this.dbContext.Projects.FirstOrDefaultAsync();
            Assert.That(createdProject, Is.Not.Null);
            Assert.That(createdProject!.Name, Is.EqualTo("New Project"));
            Assert.That(createdProject.CreatorId, Is.EqualTo(Guid.Parse(userId)));
        }

        [Test]
        public async System.Threading.Tasks.Task SoftDeleteProjectAsync_ShouldMarkProjectAsDeleted_WhenProjectExists()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            ApplicationUser user = new ApplicationUser
            {
                Id = userId,
                UserName = "TestUser",
                FullName = "TestUser"
            };

            Guid projectId = Guid.NewGuid();
            Project project = new Project
            {
                Id = projectId,
                Name = "Test Project",
                Description = "Test Description",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(5),
                CreatorId = userId,
                Creator = user,
                Status = ProjectStatus.InProgress,
                IsDeleted = false
            };

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.Projects.AddAsync(project);
            await this.dbContext.SaveChangesAsync();

            // Act
            bool result = await this.projectService.SoftDeleteProjectAsync(projectId.ToString());

            // Assert
            Assert.That(result, Is.True);
            Assert.That(this.dbContext.Projects.First().IsDeleted, Is.True);
        }

        [Test]
        public async System.Threading.Tasks.Task GetProjectByIdAsync_ShouldReturnProject_WhenProjectExists()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            ApplicationUser user = new ApplicationUser
            {
                Id = userId,
                UserName = "TestUser",
                FullName = "TestUser"
            };

            Guid projectId = Guid.NewGuid();
            Project project = new Project
            {
                Id = projectId,
                Name = "Test Project",
                Description = "Test Description",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(5),
                CreatorId = userId,
                Creator = user,
                Status = ProjectStatus.InProgress,
                IsDeleted = false,
                Milestones = new List<Milestone> { new Milestone { Name = "Milestone 1", Deadline = DateTime.UtcNow.AddDays(2) } }
            };

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.Projects.AddAsync(project);
            await this.dbContext.SaveChangesAsync();

            // Act
            Project result = await this.projectService.GetProjectByIdAsync(projectId.ToString());

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Test Project"));
            Assert.That(result.Milestones.Count, Is.EqualTo(1));
        }


        [TearDown]
        public void TearDown()
        {
            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Dispose();
        }
    }
}
