using System.Globalization;
using Microsoft.EntityFrameworkCore;

using ProjectHub.Data;
using ProjectHub.Services.Data.Interfaces;
using ProjectHub.Web.ViewModels.Task;
using ProjectHub.Data.Models;
using static ProjectHub.Common.GeneralApplicationConstants;
using ProjectHub.Web.ViewModels.Project;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProjectHub.Services.Data
{
    public class TaskService : BaseService, ITaskService
    {
        private readonly ProjectHubDbContext dbContext;

        public TaskService(ProjectHubDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> CreateTaskAsync(TaskCreateInputModel model)
        {
            Guid projectGuid = Guid.Empty;
            bool isProjectGuidValid = IsGuidValid(model.ProjectId, ref projectGuid);

            if (!isProjectGuidValid)
            {
                return false;   
            }

            Guid userGuid = Guid.Empty;
            bool isUserGuidValid = IsGuidValid(model.AssignedToUserId, ref userGuid);

            if (!isUserGuidValid)
            {
                return false;
            }

            Guid milestoneGuid = Guid.Empty;
            bool isMilestoneGuidValid = IsGuidValid(model.MilestoneId, ref milestoneGuid);

            if (!isMilestoneGuidValid)
            {
                return false;
            }

            bool isDueDateFormatValid = DateTime.TryParseExact(model.DueDate, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None,
                out DateTime dueDate);

            if (!isDueDateFormatValid)
            {
                return false;
            }

            Project? project = await this.dbContext.Projects
                 .Where(p => p.Id == projectGuid)
                 .FirstOrDefaultAsync();

            if (project == null)
            {
                return false;
            }

            if (dueDate < project.StartDate || dueDate > project.EndDate)
            {
                return false;
            }

            ProjectHub.Data.Models.Task task = new ProjectHub.Data.Models.Task()
            {
                Title = model.Title,
                Description = model.Description,
                DueDate = dueDate,
                Priority = model.Priority,
                ProjectId = projectGuid,
                MilestoneId = milestoneGuid,
                AssignedToUserId = userGuid
            };

            await this.dbContext.Tasks.AddAsync(task);
            await this.dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<ProjectHub.Data.Models.Task> GetTaskByIdAsync(string taskId)
        {
            Guid taskGuid = Guid.Empty;
            bool isTaskGuidValid = IsGuidValid(taskId, ref taskGuid);

            ProjectHub.Data.Models.Task task = await this.dbContext.Tasks
                .FirstOrDefaultAsync(t => t.Id == taskGuid && !t.IsDeleted);

            if (task == null)
            {
                throw new KeyNotFoundException($"Project with ID {taskId} not found.");
            }

            return task;
        }

        public async Task<IEnumerable<IEnumerable<IGrouping<string, TaskIndexViewModel>>>> GetGroupedTasksAssignedToUserAsync(string userId)
        {
            Guid userGuid = Guid.Empty;
            bool isUserGuidValid = IsGuidValid(userId, ref userGuid);

            var tasks = await this.dbContext.Tasks
                .Where(t => t.AssignedToUserId == userGuid && !t.IsDeleted && !t.IsCompleted)
                .Include(t => t.Milestone)
                .Include(t => t.Project)
                .Select(t => new TaskIndexViewModel
                {
                    Id = t.Id.ToString(),
                    Title = t.Title,
                    Description = t.Description,
                    DueDate = t.DueDate.ToString(DateFormat),
                    Priority = t.Priority.ToString(),
                    ProjectId = t.ProjectId.ToString(),
                    ProjectName = t.Project.Name,
                    MilestoneName = t.Milestone.Name,
                    MilestoneId = t.MilestoneId.ToString()
                })
                .ToListAsync();



            IEnumerable<IEnumerable<IGrouping<string, TaskIndexViewModel>>> groupedTasks = tasks
                    .GroupBy(t => t.ProjectName) // Group tasks by ProjectName
                    .Select(projectGroup =>
                        projectGroup
                            .GroupBy(t => t.MilestoneName) // Group tasks within each project by MilestoneName
                            .AsEnumerable()) // Make sure the result is enumerable
                    .AsEnumerable();

            return groupedTasks;
        }

        public async Task<List<ProjectHub.Data.Models.Task>> GetTasksByProjectIdAsync(string projectId)
        {
            Guid projectGuid = Guid.Empty;
            bool isProjectGuidValid = IsGuidValid(projectId, ref projectGuid);

            List<ProjectHub.Data.Models.Task> tasks = await dbContext.Tasks
                .Where(t => t.ProjectId == projectGuid && !t.IsDeleted)
                .OrderBy(t => t.DueDate)
                .ToListAsync();

            return tasks;
        }

        public async Task<bool> CompleteTaskAsync(ProjectHub.Data.Models.Task task)
        {
            task.IsCompleted = true;

            await this.dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<TaskCompletedViewModel>> GetCompletedTasksByUserAsync(string userId)
        {
            Guid userGuid = Guid.Empty;
            bool isUserGuidValid = IsGuidValid(userId, ref userGuid);

            List<TaskCompletedViewModel> completedTasks = await this.dbContext.Tasks
                .Where(t => t.IsCompleted && !t.IsDeleted && t.AssignedToUserId == userGuid)
                .Include(t => t.Project)
                .Include(t => t.Milestone)
                .Select(t => new TaskCompletedViewModel
                {
                    Title = t.Title,
                    Priority = t.Priority.ToString(),
                    ProjectName = t.Project.Name,
                    MilestoneName = t.Milestone.Name
                })
                .ToListAsync();

            return completedTasks;
        }

        public async Task<List<TaskEditFormModel>> RePopulateTasksWithAvailableUsersAsync(ProjectEditFormModel model)
        {
            Guid projectGuid = Guid.Empty;
            bool isProjectGuidValid = IsGuidValid(model.ProjectId, ref projectGuid);

            Project project = await this.dbContext.Projects.FirstOrDefaultAsync(p => p.Id == projectGuid && !p.IsDeleted);
            List<ApplicationUser> teamMembers = project.TeamMembers.ToList();
            var tasks = await this.GetTasksByProjectIdAsync(model.ProjectId);

            List<TaskEditFormModel> tasksToReturn = tasks.Select(t => new TaskEditFormModel
            {
                TaskId = t.Id.ToString(),
                Title = t.Title,
                AssignedToUserId = t.AssignedToUserId.ToString() ?? string.Empty,
                AvailableUsers = teamMembers.Select(tm => new SelectListItem
                {
                    Value = tm.Id.ToString(),
                    Text = tm.UserName
                }).ToList()
            }).ToList();

            return tasksToReturn;
        }
    }
}
