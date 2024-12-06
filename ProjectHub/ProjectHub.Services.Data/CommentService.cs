
using Microsoft.EntityFrameworkCore;
using ProjectHub.Data;
using ProjectHub.Data.Models;
using ProjectHub.Services.Data.Interfaces;
using ProjectHub.Web.ViewModels.Comment;

namespace ProjectHub.Services.Data
{
	public class CommentService : BaseService, ICommentService
    {
        private readonly ProjectHubDbContext dbContext;
        public CommentService(ProjectHubDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

		public async Task<bool> AddCommentAsync(AddCommentFormModel model, string userId)
		{
			Guid taskGuid = Guid.Empty;
			bool isTaskGuidValid = IsGuidValid(model.TaskId, ref taskGuid);

			Guid userGuid = Guid.Empty;
			bool isUserGuidValid = IsGuidValid(userId, ref userGuid);

			if (!isTaskGuidValid || !isUserGuidValid)
			{
				return false;
			}

			Comment commentToAdd = new Comment()
			{
				Content = model.Content,
				TaskId = taskGuid,
				PostedByUserId = userGuid
			};

			await this.dbContext.AddAsync(commentToAdd);
			await this.dbContext.SaveChangesAsync();
			return true;
		}

        public async Task<List<Comment>> GetCommentsByTaskIdAsync(string taskId)
        {
            Guid taskGuid = Guid.Empty;
            bool isTaskGuidValid = IsGuidValid(taskId, ref taskGuid);

			List<Comment> commentsToReturn = await this.dbContext.Comments
				.Where(c => c.TaskId == taskGuid && !c.IsDeleted)
				.Include(c => c.PostedByUser)
				.ToListAsync();

			return commentsToReturn;
        }
    }
}
