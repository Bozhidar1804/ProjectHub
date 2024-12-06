
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectHub.Data;
using ProjectHub.Data.Models;
using ProjectHub.Services.Data.Interfaces;
using ProjectHub.Web.ViewModels.Comment;
using ProjectHub.Web.ViewModels.Task;

namespace ProjectHub.Services.Data
{
	public class CommentService : BaseService, ICommentService
    {
        private readonly ProjectHubDbContext dbContext;
        public CommentService(ProjectHubDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

		public async Task<AddCommentResult> AddCommentAsync(AddCommentFormModel model, string userId)
		{
			Guid taskGuid = Guid.Empty;
			bool isTaskGuidValid = IsGuidValid(model.TaskId, ref taskGuid);

			Guid userGuid = Guid.Empty;
			bool isUserGuidValid = IsGuidValid(userId, ref userGuid);

			if (!isTaskGuidValid || !isUserGuidValid)
			{
				return new AddCommentResult { Success = false, ErrorMessage = "Invalid ID was parsed." };
            }

			Comment commentToAdd = new Comment()
			{
				Content = model.Content,
				TaskId = taskGuid,
				PostedByUserId = userGuid
			};

			await this.dbContext.AddAsync(commentToAdd);
			await this.dbContext.SaveChangesAsync();

            return new AddCommentResult { Success = true, TaskId = model.TaskId };
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
