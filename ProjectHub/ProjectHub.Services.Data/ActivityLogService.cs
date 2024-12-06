using Microsoft.EntityFrameworkCore;

using ProjectHub.Data;
using ProjectHub.Data.Models;
using ProjectHub.Data.Models.Enums;
using ProjectHub.Services.Data.Interfaces;

namespace ProjectHub.Services.Data
{
    public class ActivityLogService : BaseService, IActivityLogService
    {
        private readonly ProjectHubDbContext dbContext;
        public ActivityLogService(ProjectHubDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ActivityLog> GetActivityLogByIdAsync(string activityLogId)
        {
            Guid activityLogGuid = Guid.Empty;
            bool isActivityLogGuidValid = IsGuidValid(activityLogId, ref activityLogGuid);

            ActivityLog activityLog = await this.dbContext.ActivityLogs
                .FirstOrDefaultAsync(al => al.Id == activityLogGuid && !al.IsDeleted);

            if (activityLog == null)
            {
                throw new KeyNotFoundException($"Project with ID {activityLogId} not found.");
            }

            return activityLog;
        }

        public async Task<List<ActivityLog>> GetActivityLogsByProjectIdAsync(string projectId)
        {
            Guid projectGuid = Guid.Empty;
            bool isProjectGuidValid = IsGuidValid(projectId, ref projectGuid);

            List<ActivityLog> activityLogs = await dbContext.ActivityLogs
                .Where(al => al.Task.ProjectId == projectGuid && !al.IsDeleted)
                .OrderByDescending(al => al.Timestamp)
                .ToListAsync();

            return activityLogs;
        }

        public async System.Threading.Tasks.Task LogActionAsync(TaskAction action, string taskId, string userId)
        {
            Guid taskGuid = Guid.Empty;
            bool isTaskGuidValid = IsGuidValid(taskId, ref taskGuid);

            Guid userGuid = Guid.Empty;
            bool isUserGuidValid = IsGuidValid(userId, ref userGuid);

            if (isTaskGuidValid && isUserGuidValid)
            {
                ActivityLog log = new ActivityLog
                {
                    Action = action,
                    Timestamp = DateTime.UtcNow,
                    TaskId = taskGuid,
                    UserId = userGuid
                };

                await this.dbContext.ActivityLogs.AddAsync(log);
                await this.dbContext.SaveChangesAsync();
            }
        }
    }
}
