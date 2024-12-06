using Microsoft.EntityFrameworkCore;
using ProjectHub.Data;
using ProjectHub.Data.Models;
using ProjectHub.Services.Data.Interfaces;

namespace ProjectHub.Services.Data
{
    public class VoteService : BaseService, IVoteService
    {
        private readonly ProjectHubDbContext dbContext;
        public VoteService(ProjectHubDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> CanVoteAsync(Guid commentGuid, Guid userGuid)
        {
            // Check if the user has already voted on this comment
            return !await this.dbContext.Votes
                .AnyAsync(v => v.CommentId == commentGuid && v.UserId == userGuid);
        }

        public async System.Threading.Tasks.Task VoteUpvoteForCommentAsync(string commentId, string userId)
        {
            Guid commentGuid = Guid.Empty;
            bool isCommentGuidValid = IsGuidValid(commentId, ref commentGuid);
            Guid userGuid = Guid.Empty;
            bool isUserGuidValid = IsGuidValid(userId, ref userGuid);

            if (isCommentGuidValid && isUserGuidValid)
            {
                Comment comment = await this.dbContext.Comments.FirstOrDefaultAsync(c => c.Id == commentGuid);

                Vote existingVote = await this.dbContext.Votes
                .FirstOrDefaultAsync(v => v.CommentId == commentGuid && v.UserId == userGuid);

                if (existingVote != null)
                {
                    if (!existingVote.IsUpvote)
                    {
                        // User previously downvoted; switch to upvote
                        existingVote.IsUpvote = true;
                        comment.Downvotes--;
                        comment.Upvotes++;
                        await this.dbContext.SaveChangesAsync();
                    }
                    return;
                }

                // User has not voted; add a new upvote
                Vote newVote = new Vote
                {
                    CommentId = commentGuid,
                    UserId = userGuid,
                    IsUpvote = true,
                    CreatedOn = DateTime.UtcNow
                };

                await this.dbContext.Votes.AddAsync(newVote);
                comment.Upvotes++;
                await this.dbContext.SaveChangesAsync();
            }
        }

        public async System.Threading.Tasks.Task VoteDownvoteForCommentAsync(string commentId, string userId)
        {
            Guid commentGuid = Guid.Empty;
            bool isCommentGuidValid = IsGuidValid(commentId, ref commentGuid);
            Guid userGuid = Guid.Empty;
            bool isUserGuidValid = IsGuidValid(userId, ref userGuid);

            if (isCommentGuidValid && isUserGuidValid)
            {
                Comment comment = await this.dbContext.Comments.FirstOrDefaultAsync(c => c.Id == commentGuid);

                Vote existingVote = await this.dbContext.Votes
                .FirstOrDefaultAsync(v => v.CommentId == commentGuid && v.UserId == userGuid);

                if (existingVote != null)
                {
                    if (existingVote.IsUpvote)
                    {
                        // User previously upvoted; switch to downvote
                        existingVote.IsUpvote = false;
                        comment.Upvotes--;
                        comment.Downvotes++;
                        await this.dbContext.SaveChangesAsync();
                    }
                    return;
                }

                // User has not voted; add a new downvote
                Vote newVote = new Vote
                {
                    CommentId = commentGuid,
                    UserId = userGuid,
                    IsUpvote = false,
                    CreatedOn = DateTime.UtcNow
                };

                await this.dbContext.Votes.AddAsync(newVote);
                comment.Downvotes++;
                await this.dbContext.SaveChangesAsync();
            }
        }
    }
}
