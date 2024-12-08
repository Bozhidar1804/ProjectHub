using Microsoft.EntityFrameworkCore;

using ProjectHub.Common;
using ProjectHub.Data;
using ProjectHub.Data.Models;
using ProjectHub.Data.Models.Enums;
using ProjectHub.Services.Data;
using ProjectHub.Services.Data.Interfaces;
using ProjectHub.Web.ViewModels.Task;

namespace ProjectHub.Services.Tests
{
    [TestFixture]
    public class TaskServiceTests
    {
        private ProjectHubDbContext dbContext;
        private ITaskService taskService;
        private Project setUpProject;
        private Milestone setUpMilestone;
        private ApplicationUser setUpUser;

        [SetUp]
        public async System.Threading.Tasks.Task SetUp()
        {
            var options = new DbContextOptionsBuilder<ProjectHubDbContext>()
                .UseInMemoryDatabase("TaskServiceTestDb")
                .Options;

            dbContext = new ProjectHubDbContext(options);
            taskService = new TaskService(dbContext);

            setUpProject = new Project
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

            setUpMilestone = new Milestone
            {
                Id = Guid.NewGuid(),
                Name = "Test Milestone",
                ProjectId = setUpProject.Id
            };

            setUpUser = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                Email = "user1@example.com",
                FullName = "User One"
            };

            dbContext.Projects.Add(setUpProject);
            dbContext.Milestones.Add(setUpMilestone);
            dbContext.Users.Add(setUpUser);
            await dbContext.SaveChangesAsync();
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();
        }

        [Test]
        public async System.Threading.Tasks.Task GetTaskByIdAsync_ShouldReturnTask_WhenTaskExists()
        {
            // Arrange
            var task = new ProjectHub.Data.Models.Task
            {
                Title = "Test Task",
                Description = "Test Description",
                DueDate = DateTime.UtcNow,
                Priority = TaskPriority.High,
                IsCompleted = false,
                IsDeleted = false,
                ProjectId = Guid.NewGuid(),
                MilestoneId = Guid.NewGuid(),
                AssignedToUserId = Guid.NewGuid()
            };

            dbContext.Tasks.Add(task);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await taskService.GetTaskByIdAsync(task.Id.ToString());

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Title, Is.EqualTo("Test Task"));
        }

        [Test]
        public async System.Threading.Tasks.Task GetTasksByProjectIdAsync_ShouldReturnTasks_WhenTasksExist()
        {
            // Arrange
            Guid projectId = Guid.NewGuid();
            var task1 = new ProjectHub.Data.Models.Task
            {
                Title = "Task 1",
                ProjectId = projectId,
                Description = "TaskDescr",
                IsDeleted = false
            };
            var task2 = new ProjectHub.Data.Models.Task
            {
                Title = "Task 2",
                ProjectId = projectId,
                Description = "TaskDescr",
                IsDeleted = false
            };

            dbContext.Tasks.AddRange(task1, task2);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await taskService.GetTasksByProjectIdAsync(projectId.ToString());

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Title, Is.EqualTo("Task 2"));
            Assert.That(result[1].Title, Is.EqualTo("Task 1"));
        }

        [Test]
        public async System.Threading.Tasks.Task GetTaskTitleAsync_ShouldReturnTitle_WhenTaskExists()
        {
            // Arrange
            var task = new ProjectHub.Data.Models.Task
            {
                Title = "Sample Title",
                Description = "TaskDescr",
                IsDeleted = false
            };

            dbContext.Tasks.Add(task);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await taskService.GetTaskTitleAsync(task.Id.ToString());

            // Assert
            Assert.That(result, Is.EqualTo("Sample Title"));
        }

        [Test]
        public async System.Threading.Tasks.Task CreateTaskAsync_ShouldCreateTaskSuccessfully_WhenValidInputIsProvided()
        {
            // Arrange
            TaskCreateInputModel inputModel = new TaskCreateInputModel
            {
                Title = "Test Task",
                Description = "Test Description",
                DueDate = DateTime.UtcNow.AddDays(10).ToString(GeneralApplicationConstants.DateFormat),
                Priority = TaskPriority.High,
                ProjectId = setUpProject.Id.ToString(),
                MilestoneId = setUpMilestone.Id.ToString(),
                AssignedToUserId = setUpUser.Id.ToString()
            };

            // Act
            TaskCreateResult result = await taskService.CreateTaskAsync(inputModel);

            // Assert
            Assert.IsTrue(result.Success, "Task creation should be successful.");
            Assert.IsNotNull(result.TaskId, "Result should return a Task ID.");

            var createdTask = await dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == Guid.Parse(result.TaskId));
            Assert.IsNotNull(createdTask, "Created task should exist in the database.");
            Assert.That(createdTask.Title, Is.EqualTo("Test Task"));
        }

        [Test]
        public async System.Threading.Tasks.Task GetCompletedTasksByUserAsync_ShouldReturnCompletedTasks_WhenUserIdIsValid()
        {
            // Arrange
            var completedTask = new ProjectHub.Data.Models.Task
            {
                Id = Guid.NewGuid(),
                Title = "Completed Task",
                Description = "TaskDescr",
                Priority = TaskPriority.Medium,
                IsCompleted = true,
                IsDeleted = false,
                AssignedToUserId = setUpUser.Id,
                ProjectId = setUpProject.Id,
                MilestoneId = setUpMilestone.Id
            };
            await this.dbContext.AddAsync(completedTask);
            await this.dbContext.SaveChangesAsync();

            // Act
            var result = await taskService.GetCompletedTasksByUserAsync(setUpUser.Id.ToString());

            // Assert
            Assert.IsNotNull(result, "Result should not be null.");
            Assert.That(result.Count, Is.EqualTo(1));
            var returnedTask = result.First();
            Assert.That(returnedTask.Title, Is.EqualTo("Completed Task"), "Returned task title should match.");
        }
    }
}
