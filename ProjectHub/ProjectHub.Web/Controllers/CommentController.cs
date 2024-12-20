﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using ProjectHub.Services.Data.Interfaces;
using ProjectHub.Data.Models;
using ProjectHub.Web.ViewModels.Comment;
using ProjectHub.Web.Infrastructure.Extensions;
using ProjectHub.Web.ViewModels.Task;
using static ProjectHub.Common.GeneralApplicationConstants;
using ProjectHub.Data.Models.Enums;

namespace ProjectHub.Web.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IProjectService projectService;
        private readonly ITaskService taskService;
		private readonly ICommentService commentService;
        private readonly IVoteService voteService;
        private readonly IActivityLogService activityLogService;

		public CommentController(UserManager<ApplicationUser> userManager, IProjectService projectService, ITaskService taskService, ICommentService commentService, IVoteService voteService, IActivityLogService activityLogService)
        {
            this.userManager = userManager;
            this.projectService = projectService;
            this.taskService = taskService;
            this.commentService = commentService;
            this.voteService = voteService;
            this.activityLogService = activityLogService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string taskId)
        {
            List<Comment> comments = await this.commentService.GetCommentsByTaskIdAsync(taskId);
            string taskTitle = await this.taskService.GetTaskTitleAsync(taskId);

            TaskCommentsViewModel model = new TaskCommentsViewModel
            {
                TaskId = taskId,
                TaskTitle = taskTitle,
                Comments = comments.Select(c => new CommentViewModel
                {
                    CommentId = c.Id.ToString(),
                    Content = c.Content,
                    AuthorName = c.PostedByUser.FullName,
                    CreatedOn = c.CreatedOn.ToString(DateFormat),
                    Upvotes = c.Upvotes,
                    Downvotes = c.Downvotes
                }).ToList()
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddComment(string taskId)
        {
            try
            {
                string taskTitle = await this.taskService.GetTaskTitleAsync(taskId);

                AddCommentFormModel viewModel = new AddCommentFormModel
                {
                    TaskId = taskId,
                    TaskTitle = taskTitle
                };

                return View(viewModel);
            }
            catch (ArgumentException ex)
            {
                // Handle cases where the task doesn't exist
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Index", "Task");
            }
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddComment(AddCommentFormModel model)
		{
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while adding the comment!");
                return View(model);
            }

			try
			{
                string userId = this.User.GetUserId()!;

                AddCommentResult addCommentResult = await this.commentService.AddCommentAsync(model, userId);

				if (!addCommentResult.Success)
				{
					ModelState.AddModelError(string.Empty, "An error occurred while adding the comment.");
					return View(model);
				}

                await this.activityLogService.LogActionAsync(TaskAction.CommentAdded, addCommentResult.TaskId!, userId);

                return RedirectToAction("Index", "Task");
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, "An error occurred while adding the comment.");
				return View(model);
			}
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upvote(string commentId, string taskId)
        {
            string userId = this.User.GetUserId()!;

            if (!string.IsNullOrEmpty(taskId) && !string.IsNullOrEmpty(userId))
            {
                await this.voteService.VoteUpvoteForCommentAsync(commentId, userId);
            }

            return RedirectToAction(nameof(Index), new { taskId = taskId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Downvote(string commentId, string taskId)
        {
            string userId = this.User.GetUserId()!;

            if (!string.IsNullOrEmpty(taskId) && !string.IsNullOrEmpty(userId))
            {
                await this.voteService.VoteDownvoteForCommentAsync(commentId, userId);
            }

            return RedirectToAction(nameof(Index), new { taskId = taskId });
        }

    }
}
