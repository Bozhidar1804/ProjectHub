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
