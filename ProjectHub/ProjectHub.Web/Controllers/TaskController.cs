﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

using ProjectHub.Services.Data.Interfaces;
using ProjectHub.Data.Models;
using static ProjectHub.Common.GeneralApplicationConstants;
using ProjectHub.Web.ViewModels.Task;
using ProjectHub.Data.Models.Enums;
using ProjectHub.Web.Infrastructure.Extensions;

namespace ProjectHub.Web.Controllers
{
    [Authorize]
	public class TaskController : Controller
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly IProjectService projectService;
		private readonly IMilestoneService milestoneService;
		private readonly ITaskService taskService;
        private readonly IUserService userService;
        private readonly IActivityLogService activityLogService;

		public TaskController(UserManager<ApplicationUser> userManager, IProjectService projectService, IMilestoneService milestoneService, ITaskService taskService, IUserService userService, IActivityLogService activityLogService)
		{
			this.userManager = userManager;
			this.projectService = projectService;
			this.milestoneService = milestoneService;
			this.taskService = taskService;
            this.userService = userService;
            this.activityLogService = activityLogService;
		}

        [HttpGet]
		public async Task<IActionResult> Index()
		{
            string userId = this.User.GetUserId()!;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            IEnumerable<IEnumerable<IGrouping<string, TaskIndexViewModel>>> tasksGrouped = await this.taskService.GetGroupedTasksAssignedToUserAsync(userId);

            return View(tasksGrouped);
        }

        [HttpGet]
        [Authorize(Roles = ModeratorRoleName)]
        public async Task<IActionResult> Create(string projectId)
        {
            Project project = await this.projectService.GetProjectByIdAsync(projectId);

            if (project == null)
            {
                return NotFound();
            }

            ICollection<ApplicationUser> projectMembers = project.TeamMembers;

            List<SelectListItem> userSelectList = projectMembers.Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = u.UserName
            }).ToList();

            List<SelectListItem> prioritiesSelectList = Enum
                .GetValues(typeof(TaskPriority))
                .Cast<TaskPriority>()
                .Select(p => new SelectListItem
                {
                    Value = p.ToString(),
                    Text = p.ToString()
                }).ToList();

            List<SelectListItem> milestones = project.Milestones.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Name
            }).ToList();

            TaskCreateInputModel model = new TaskCreateInputModel()
            {
                ProjectId = projectId,
                Users = userSelectList,
                Priorities = prioritiesSelectList,
                Milestones = milestones
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = ModeratorRoleName)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskCreateInputModel model)
        {
            if (!ModelState.IsValid)
            {
                this.ModelState.AddModelError(string.Empty, "An error occurred while creating the task!");

                await LoadProjectUsersAsync(model);

                return View(model);
            }

            TaskCreateResult taskCreateResult = await this.taskService.CreateTaskAsync(model);

            if (!taskCreateResult.Success)
            {
                this.ModelState.AddModelError(string.Empty, "An error occurred while creating the task!");

                await LoadProjectUsersAsync(model);

                return View(model);
            }

            string userId = this.User.GetUserId()!;
            await this.activityLogService.LogActionAsync(TaskAction.Created, taskCreateResult.TaskId!, userId);

            return RedirectToAction("Manage", "Project", new { projectId = model.ProjectId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete(string taskId)
        {
            var task = await this.taskService.GetTaskByIdAsync(taskId);

            if (task == null)
            {
                return NotFound();
            }

            bool isCompletedResult = await this.taskService.CompleteTaskAsync(task);
            if (!isCompletedResult)
            {
                return BadRequest();
            }

            string userId = this.User.GetUserId()!;
            await this.activityLogService.LogActionAsync(TaskAction.Completed, taskId, userId);

            return RedirectToAction(nameof(DisplayCompletedTasks));
        }

        [HttpGet]
        public async Task<IActionResult> DisplayCompletedTasks()
        {
            string userId = this.User.GetUserId()!;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            List<TaskCompletedViewModel> completedTasks = await this.taskService.GetCompletedTasksByUserAsync(userId);

            return View(completedTasks);
        }


        private async System.Threading.Tasks.Task LoadProjectUsersAsync(TaskCreateInputModel model)
        {
            Project project = await this.projectService.GetProjectByIdAsync(model.ProjectId);
            ICollection<ApplicationUser> projectMembers = project.TeamMembers;

            model.Users = projectMembers.Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = u.UserName
            }).ToList();
        }
    }
}
