using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

using ProjectHub.Services.Data.Interfaces;
using ProjectHub.Data.Models;
using static ProjectHub.Common.GeneralApplicationConstants;
using ProjectHub.Web.ViewModels.Task;
using ProjectHub.Data.Models.Enums;
using ProjectHub.Services.Data;
using ProjectHub.Web.Infrastructure.Extensions;

namespace ProjectHub.Web.Controllers
{
	public class TaskController : Controller
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly IProjectService projectService;
		private readonly IMilestoneService milestoneService;
		private readonly ITaskService taskService;
        private readonly IUserService userService;

		public TaskController(UserManager<ApplicationUser> userManager, IProjectService projectService, IMilestoneService milestoneService, ITaskService taskService, IUserService userService)
		{
			this.userManager = userManager;
			this.projectService = projectService;
			this.milestoneService = milestoneService;
			this.taskService = taskService;
            this.userService = userService;
		}

        [HttpGet]
		public async Task<IActionResult> Index()
		{
            string userId = this.User.GetUserId()!;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            IEnumerable<TaskIndexViewModel> tasks = await this.taskService.GetTasksAssignedToUserAsync(userId);

            IEnumerable<IGrouping<string, TaskIndexViewModel>> groupedTasks = tasks
                .GroupBy(t => t.ProjectName)
                .ToList();

            return View(groupedTasks);
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

            TaskCreateFormModel model = new TaskCreateFormModel()
            {
                ProjectId = projectId,
                Users = userSelectList,
                Priorities = prioritiesSelectList
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = ModeratorRoleName)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskCreateFormModel model)
        {
            if (!ModelState.IsValid)
            {
                this.ModelState.AddModelError(string.Empty, "An error occurred while creating the task!");

                await LoadProjectUsersAsync(model);

                return View(model);
            }

            bool isAddedResult = await this.taskService.CreateTaskAsync(model);

            if (!isAddedResult)
            {
                this.ModelState.AddModelError(string.Empty, "An error occurred while creating the task!");

                await LoadProjectUsersAsync(model);

                return View(model);
            }

            return RedirectToAction("Manage", "Project", new { projectId = model.ProjectId });
        }

        private async System.Threading.Tasks.Task LoadProjectUsersAsync(TaskCreateFormModel model)
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
