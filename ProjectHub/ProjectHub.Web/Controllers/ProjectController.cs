using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using ProjectHub.Data.Models;
using ProjectHub.Services.Data.Interfaces;
using ProjectHub.Web.Infrastructure.Extensions;
using ProjectHub.Web.ViewModels.Project;
using ProjectHub.Web.ViewModels.ActivityLog;
using static ProjectHub.Web.Infrastructure.Extensions.ClaimsPrincipalExtensions;
using static ProjectHub.Common.GeneralApplicationConstants;
using ProjectHub.Web.ViewModels.Milestone;
using ProjectHub.Web.ViewModels.Task;

namespace ProjectHub.Web.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IProjectService projectService;
        private readonly IMilestoneService milestoneService;
        private readonly ITaskService taskService;
        private readonly ITagService tagService;
        private readonly IActivityLogService activityLogService;

        public ProjectController(UserManager<ApplicationUser> userManager, IProjectService projectService, IMilestoneService milestoneService, ITaskService taskService, ITagService tagService, IActivityLogService activityLogService)
        {
            this.userManager = userManager;
            this.projectService = projectService;
            this.milestoneService = milestoneService;
            this.taskService = taskService;
            this.tagService = tagService;
            this.activityLogService = activityLogService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<ProjectIndexViewModel> projectsModels = await this.projectService.GetAllProjectsAsync();

            return View(projectsModels);
        }

        [HttpGet]
        [Authorize(Roles = ModeratorRoleName)]
        public async Task<IActionResult> MyProjects()
        {
            string userId = this.User.GetUserId()!;
            IEnumerable<ProjectIndexViewModel> myProjectsModels = await this.projectService.GetCreatorAllProjectsAsync(userId);

            return View(myProjectsModels);
        }

        [HttpGet]
        [Authorize(Roles = ModeratorRoleName)]
        public IActionResult Create()
        {
            ProjectCreateFormModel model = new ProjectCreateFormModel();

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = ModeratorRoleName)]
        public async Task<IActionResult> Create(ProjectCreateFormModel model)
        {
            if (!ModelState.IsValid)
            {
                this.ModelState.AddModelError(string.Empty, "An error occurred while creating the project!");
                return View(model);
            }

            string userId = ClaimsPrincipalExtensions.GetUserId(User)!;
            bool isAddedResult = await projectService.CreateProjectAsync(model, userId);

            if (!isAddedResult)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while creating the project.");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
		public async Task<IActionResult> Delete(string Id)
		{
			Project project = await this.projectService.GetProjectByIdAsync(Id);

            ProjectDeleteViewModel projectModel = new ProjectDeleteViewModel()
            {
                Id = project.Id.ToString(),
                Name = project.Name,
                Description = project.Description,
                EndDate = project.EndDate.ToString(DateFormat),
                Status = project.Status,
                IsDeleted = project.IsDeleted
            };

            if (projectModel == null || projectModel.IsDeleted)
			{
				return NotFound();
			}

			return View(projectModel);
		}

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string Id)
        {
            bool result = await this.projectService.SoftDeleteProjectAsync(Id);

            if (!result)
            {
                return BadRequest("Unable to delete the project. Please try again.");
            }

            return RedirectToAction(nameof(MyProjects));
        }

        [HttpGet]
        [Authorize(Roles = ModeratorRoleName)]
        public async Task<IActionResult> Manage(string projectId)
        {
            try
            {
                Project project = await projectService.GetProjectByIdAsync(projectId);
                if (project == null)
                {
                    return NotFound();
                }

                List<Milestone> milestones = await this.milestoneService.GetMilestonesByProjectIdAsync(projectId);
                if (milestones == null)
                {
                    milestones = new List<Milestone>(); // Empty list if no milestones found
                }

                List<Data.Models.Task> tasks = await this.taskService.GetTasksByProjectIdAsync(projectId);
                if (tasks == null)
                {
                    tasks = new List<Data.Models.Task>(); // Empty list if no tasks found
                }

                List<string> tagsForProject = await this.tagService.GetTagsByProjectIdAsync(projectId);
                List<ActivityLog> activityLogsByProject = await this.activityLogService.GetActivityLogsByProjectIdAsync(projectId);

                ProjectManageViewModel projectViewModel = new ProjectManageViewModel()
                {
                    ProjectId = projectId,
                    Name = project.Name,
                    Description = project.Description,
                    Members = project.TeamMembers.Select(tm => new ProjectMemberViewModel()
                    {
                        UserId = tm.Id.ToString(),
                        UserName = tm.UserName!
                    }).ToList(),
                    Milestones = milestones.Select(m => new MilestoneViewModel
                    {
                        Id = m.Id.ToString(),
                        Name = m.Name,
                        Deadline = m.Deadline.ToString(DateFormat)
                    }).ToList(),
                    Tasks = tasks.Select(t => new TaskViewModel
                    {
                        Id = t.Id.ToString(),
                        Title = t.Title,
                        Priority = t.Priority.ToString(),
                        AssignedTo = t.AssignedToUser?.UserName ?? "Unassigned",
                        ActivityLogs = activityLogsByProject.Select(al => new ActivityLogViewModel
                        {
                            Id = al.Id.ToString(),
                            Action = al.Action.ToString(),
                            PerformedBy = al.User.UserName ?? "error",
                            Timestamp = al.Timestamp.ToString(DateFormat)
                        }).ToList()
                    }).ToList(),
                };

                return View(projectViewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while processing the request.";
                return RedirectToAction(nameof(MyProjects));
            }
        }
    }
}
