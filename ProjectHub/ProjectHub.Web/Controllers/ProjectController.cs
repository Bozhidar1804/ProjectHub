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
using ProjectHub.Services.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProjectHub.Web.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IProjectService projectService;
        private readonly IMilestoneService milestoneService;
        private readonly ITaskService taskService;
        private readonly IActivityLogService activityLogService;

        public ProjectController(UserManager<ApplicationUser> userManager, IProjectService projectService, IMilestoneService milestoneService, ITaskService taskService, IActivityLogService activityLogService)
        {
            this.userManager = userManager;
            this.projectService = projectService;
            this.milestoneService = milestoneService;
            this.taskService = taskService;
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
            ProjectCreateInputModel model = new ProjectCreateInputModel();

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = ModeratorRoleName)]
        public async Task<IActionResult> Create(ProjectCreateInputModel model)
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
        public async Task<IActionResult> Details(string projectId)
        {
            Project project = await this.projectService.GetProjectByIdAsync(projectId);
            bool isUserPartOfProject = (User.IsInRole(UserRoleName) || User.IsInRole(ModeratorRoleName)) && project.TeamMembers.Any(tm => tm.UserName == User.Identity?.Name);

            ProjectDetailsViewModel projectViewModel = new ProjectDetailsViewModel
            {
                ProjectId = project.Id.ToString(),
                Name = project.Name,
                Description = project.Description,
                StartDate = project.StartDate.ToString(DateFormat),
                EndDate = project.EndDate.ToString(DateFormat),
                Status = project.Status,
                CreatorName = project.Creator.UserName!,
                TeamMemberCount = project.TeamMembers.Count,
                MaxMilestones = project.MaxMilestones ?? 0,
                Members = project.TeamMembers.Select(tm => new ProjectMemberViewModel
                {
                    UserId = tm.Id.ToString(),
                    UserName = tm.UserName,
                    Role = tm.Id == project.CreatorId ? "Creator" : "Member",
                    CompletedTasks = project.Tasks.Count(t => t.AssignedToUserId == tm.Id && t.IsCompleted)
                }).ToList()
            };

            if (isUserPartOfProject)
            {
                List<Milestone> milestonesForProject = await this.milestoneService.GetMilestonesByProjectIdAsync(projectId);
                List<Data.Models.Task> tasksForProject = await this.taskService.GetTasksByProjectIdAsync(projectId);

                projectViewModel.Milestones = milestonesForProject.Select(m => new MilestoneProjectDetailsViewModel
                {
                    Id = m.Id.ToString(),
                    Name = m.Name,
                    Deadline = m.Deadline.ToString(),
                    IsCompleted = m.IsCompleted,
                    Progress = m.Progress
                }).ToList();

                projectViewModel.Tasks = tasksForProject.Select(t => new TaskProjectDetailsViewModel
                {
                    Id = t.Id.ToString(),
                    Title = t.Title,
                    AssignedTo = t.AssignedToUser?.UserName ?? "Unassigned",
                    MilestoneName = t.Milestone?.Name ?? "No Milestone",
                    Priority = t.Priority.ToString(),
                    IsCompleted = t.IsCompleted
                }).ToList();
            }

            return View(projectViewModel);
        }

        [HttpGet]
        [Authorize(Roles = ModeratorRoleName)]
        public async Task<IActionResult> Edit(string projectId)
        {
            Project project = await this.projectService.GetProjectByIdAsync(projectId);

            if (project == null)
            {
                return NotFound();
            }

            List<ApplicationUser> teamMembers = project.TeamMembers.ToList();
            var tasks = await this.taskService.GetTasksByProjectIdAsync(projectId);

            ProjectEditFormModel model = new ProjectEditFormModel()
            {
                ProjectId = project.Id.ToString(),
                Name = project.Name,
                Description = project.Description,
                EndDate = project.EndDate.ToString(DateFormat),
                MaxMilestones = project.MaxMilestones,
                Tasks = tasks.Select(t => new TaskEditFormModel
                {
                    TaskId = t.Id.ToString(),
                    Title = t.Title,
                    AssignedToUserId = t.AssignedToUserId.ToString() ?? string.Empty,
                    AvailableUsers = teamMembers.Select(tm => new SelectListItem
                    {
                        Value = tm.Id.ToString(),
                        Text = tm.UserName
                    }).ToList()
                }).ToList()
            };

            return View(model);
        }


        [HttpGet]
        [Authorize(Roles = ModeratorRoleName)]
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
        [Authorize(Roles = ModeratorRoleName)]
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

                List<ActivityLog> activityLogsByProject = await this.activityLogService.GetActivityLogsByProjectIdAsync(projectId);

                ProjectManageViewModel projectViewModel = new ProjectManageViewModel()
                {
                    ProjectId = projectId,
                    Name = project.Name,
                    Description = project.Description,
                    MaxMilestones = project.MaxMilestones,
                    Members = project.TeamMembers.Select(tm => new ProjectMemberViewModel()
                    {
                        UserId = tm.Id.ToString(),
                        UserName = tm.UserName!,
                        Role = tm.Id == project.CreatorId ? "Creator" : "Member",
                        CompletedTasks = tasks.Count(t => t.AssignedToUserId == tm.Id && t.IsCompleted)
                    }).ToList(),
                    Milestones = milestones.Select(m => new MilestoneViewModel
                    {
                        Id = m.Id.ToString(),
                        Name = m.Name,
                        Deadline = m.Deadline.ToString(DateFormat),
                        IsCompleted = m.IsCompleted,
                        Progress = m.Progress
                    }).ToList(),
                    Tasks = tasks.Select(t => new TaskViewModel
                    {
                        Id = t.Id.ToString(),
                        Title = t.Title,
                        Priority = t.Priority.ToString(),
                        AssignedTo = t.AssignedToUser?.UserName ?? "Unassigned",
                        MilestoneName = t.Milestone.Name,
                        MilestoneId = t.MilestoneId.ToString(),
                        IsCompleted = t.IsCompleted,
                        ActivityLogs = activityLogsByProject.Select(al => new ActivityLogViewModel
                        {
                            Id = al.Id.ToString(),
                            Action = al.Action.ToString(),
                            PerformedBy = al.User.UserName ?? "error",
                            Timestamp = al.Timestamp.ToString(DateFormat)
                        }).ToList()
                    }).ToList()
                };

                return View(projectViewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while processing the request.";
                return RedirectToAction(nameof(MyProjects));
            }
        }

        [HttpPost]
        [Authorize(Roles = ModeratorRoleName)]
        public async Task<IActionResult> SetMaxMilestones(string projectId, int maxMilestones)
        {
            try
            {
                if (maxMilestones < 1)
                {
                    TempData["ErrorMessage"] = "Maximum milestones must be at least 1.";
                    return RedirectToAction(nameof(Manage), new { projectId });
                }

                await projectService.SetMaxMilestonesAsync(projectId, maxMilestones);
                TempData["SuccessMessage"] = "Max milestones set successfully.";
                return RedirectToAction(nameof(Manage), new { projectId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while setting max milestones.";
                return RedirectToAction(nameof(Manage), new { projectId });
            }
        }
    }
}
