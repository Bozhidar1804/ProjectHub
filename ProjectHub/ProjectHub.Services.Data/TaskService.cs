using Microsoft.EntityFrameworkCore;

using ProjectHub.Data;
using ProjectHub.Data.Models;
using ProjectHub.Services.Data.Interfaces;

namespace ProjectHub.Services.Data
{
    public class TaskService : BaseService, ITaskService
    {
        private readonly ProjectHubDbContext dbContext;

        public TaskService(ProjectHubDbContext dbContext)
        {
            this.dbContext = dbContext;
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
