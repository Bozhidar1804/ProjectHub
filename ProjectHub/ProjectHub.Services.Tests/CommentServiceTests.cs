using Microsoft.EntityFrameworkCore;

using ProjectHub.Data;
using ProjectHub.Data.Models;
using ProjectHub.Services.Data.Interfaces;
using ProjectHub.Services.Data;
using ProjectHub.Web.ViewModels.Comment;

namespace ProjectHub.Services.Tests
{
    [TestFixture]
    public class CommentServiceTests
    {
        private ICommentService commentService;
        private ProjectHubDbContext dbContext;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ProjectHubDbContext>()
                .UseInMemoryDatabase("CommentServiceTestDb")
                .Options;

            dbContext = new ProjectHubDbContext(options);
            commentService = new CommentService(dbContext);

            // Add test data
            ApplicationUser user = new ApplicationUser { Id = Guid.NewGuid(), UserName = "user1", Email = "user1@example.com", FullName = "User1" };
            var task = new ProjectHub.Data.Models.Task { Id = Guid.NewGuid(), Title = "Test Task", Description = "Test Descr" };

            dbContext.Users.Add(user);
            dbContext.Tasks.Add(task);
            dbContext.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Dispose();
        }

        [Test]
        public async System.Threading.Tasks.Task AddCommentAsync_WithValidData_ShouldAddComment()
        {
            // Arrange
            AddCommentFormModel model = new AddCommentFormModel
            {
                TaskId = dbContext.Tasks.First().Id.ToString(),
                Content = "This is a test comment."
            };
            string userId = dbContext.Users.First().Id.ToString();

            // Act
            AddCommentResult result = await commentService.AddCommentAsync(model, userId);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.That(dbContext.Comments.Count(), Is.EqualTo(1));
        }

        [Test]
        public async System.Threading.Tasks.Task AddCommentAsync_WithInvalidTaskId_ShouldReturnError()
        {
            // Arrange
            AddCommentFormModel model = new AddCommentFormModel
            {
                TaskId = "invalid-guid",
                Content = "This is a test comment."
            };
            string userId = dbContext.Users.First().Id.ToString();

            // Act
            AddCommentResult result = await commentService.AddCommentAsync(model, userId);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.That(result.ErrorMessage, Is.EqualTo("Invalid ID was parsed."));
            Assert.That(dbContext.Comments.Count(), Is.EqualTo(0));
        }

        [Test]
        public async System.Threading.Tasks.Task GetCommentsByTaskIdAsync_WithValidTaskId_ShouldReturnComments()
        {
            // Arrange
            var task = dbContext.Tasks.First();
            Comment comment = new Comment
            {
                Content = "This is a test comment.",
                TaskId = task.Id,
                PostedByUserId = dbContext.Users.First().Id
            };

            dbContext.Comments.Add(comment);
            dbContext.SaveChanges();

            // Act
            List<Comment> comments = await commentService.GetCommentsByTaskIdAsync(task.Id.ToString());

            // Assert
            Assert.IsNotNull(comments);
            Assert.That(comments.Count, Is.EqualTo(1));
        }
    }
}
