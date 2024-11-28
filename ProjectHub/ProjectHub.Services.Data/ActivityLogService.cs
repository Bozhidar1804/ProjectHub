using Microsoft.EntityFrameworkCore;

using ProjectHub.Data;
using ProjectHub.Data.Models;
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
    }
}
