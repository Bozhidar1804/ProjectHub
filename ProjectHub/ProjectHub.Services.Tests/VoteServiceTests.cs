using Microsoft.EntityFrameworkCore;

using ProjectHub.Data;
using ProjectHub.Data.Models;
using ProjectHub.Services.Data.Interfaces;
using ProjectHub.Services.Data;

namespace ProjectHub.Services.Tests
{
    [TestFixture]
    public class VoteServiceTests
    {
        private IVoteService voteService;
        private ProjectHubDbContext dbContext;

        [SetUp]
        public async System.Threading.Tasks.Task SetUp()
        {
            // Setup in-memory database
            var options = new DbContextOptionsBuilder<ProjectHubDbContext>()
                .UseInMemoryDatabase("VoteServiceTestDb")
                .Options;

            dbContext = new ProjectHubDbContext(options);
            voteService = new VoteService(dbContext);

            // Create test users
            ApplicationUser user1 = new ApplicationUser { Id = Guid.NewGuid(), UserName = "user1", Email = "user1@example.com", FullName = "User1" };
            ApplicationUser user2 = new ApplicationUser { Id = Guid.NewGuid(), UserName = "user2", Email = "user2@example.com", FullName = "User2" };

            // Create test comment
            Comment comment = new Comment
            {
                Id = Guid.NewGuid(),
                Content = "This is a test comment.",
                PostedByUserId = user1.Id
            };

            await dbContext.Users.AddAsync(user1);
            await dbContext.Users.AddAsync(user2);
            await dbContext.Comments.AddAsync(comment);
            await dbContext.SaveChangesAsync();
        }

        [Test]
        public async System.Threading.Tasks.Task CanVoteAsync_ShouldReturnTrue_WhenUserHasNotVoted()
        {
            // Arrange
            Guid commentId = dbContext.Comments.First().Id;
            Guid userId = dbContext.Users.First().Id;

            // Act
            bool canVote = await voteService.CanVoteAsync(commentId, userId);

            // Assert
            Assert.IsTrue(canVote);
        }

        [Test]
        public async System.Threading.Tasks.Task CanVoteAsync_ShouldReturnFalse_WhenUserHasAlreadyVoted()
        {
            // Arrange
            Guid commentId = dbContext.Comments.First().Id;
            Guid userId = dbContext.Users.First().Id;

            // Simulate a vote
            var vote = new Vote
            {
                CommentId = commentId,
                UserId = userId,
                IsUpvote = true,
                CreatedOn = DateTime.UtcNow
            };
            dbContext.Votes.Add(vote);
            dbContext.SaveChanges();

            // Act
            bool canVote = await voteService.CanVoteAsync(commentId, userId);

            // Assert
            Assert.IsFalse(canVote);
        }

        [Test]
        public async System.Threading.Tasks.Task VoteUpvoteForCommentAsync_ShouldAddUpvote_WhenUserHasNotVoted()
        {
            // Arrange
            Guid commentId = dbContext.Comments.First().Id;
            Guid userId = dbContext.Users.First().Id;

            // Act
            await voteService.VoteUpvoteForCommentAsync(commentId.ToString(), userId.ToString());

            // Assert
            var vote = await dbContext.Votes.FirstOrDefaultAsync(v => v.CommentId == commentId && v.UserId == userId);
            Assert.IsNotNull(vote);
            Assert.IsTrue(vote.IsUpvote);
            Assert.That(await dbContext.Comments.Where(c => c.Id == commentId).Select(c => c.Upvotes).FirstOrDefaultAsync(), Is.EqualTo(1));
        }

        [Test]
        public async System.Threading.Tasks.Task VoteDownvoteForCommentAsync_ShouldAddDownvote_WhenUserHasNotVoted()
        {
            // Arrange
            Guid commentId = dbContext.Comments.First().Id;
            Guid userId = dbContext.Users.First().Id;

            // Act
            await voteService.VoteDownvoteForCommentAsync(commentId.ToString(), userId.ToString());

            // Assert
            var vote = await dbContext.Votes.FirstOrDefaultAsync(v => v.CommentId == commentId && v.UserId == userId);
            Assert.IsNotNull(vote);
            Assert.IsFalse(vote.IsUpvote);
            Assert.That(await dbContext.Comments.Where(c => c.Id == commentId).Select(c => c.Downvotes).FirstOrDefaultAsync(), Is.EqualTo(1));
        }

        [TearDown]
        public void TearDown()
        {
            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Dispose();
        }
    }
}
