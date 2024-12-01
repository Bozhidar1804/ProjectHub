using System.Globalization;
using Microsoft.EntityFrameworkCore;

using ProjectHub.Data;
using ProjectHub.Services.Data.Interfaces;
using ProjectHub.Web.ViewModels.Task;
using ProjectHub.Data.Models;
using static ProjectHub.Common.GeneralApplicationConstants;

namespace ProjectHub.Services.Data
{
    public class TaskService : BaseService, ITaskService
    {
        private readonly ProjectHubDbContext dbContext;

        public TaskService(ProjectHubDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> CreateTaskAsync(TaskCreateFormModel model)
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
                AssignedToUserId = userGuid,
                ProjectId = projectGuid,
                Priority = model.Priority
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
    }
}
