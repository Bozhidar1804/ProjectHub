using ProjectHub.Data.Models;

namespace ProjectHub.Services.Data.Interfaces
{
    public interface IActivityLogService
    {
        Task<ActivityLog> GetActivityLogByIdAsync(string activityLogId);

        Task<List<ActivityLog>> GetActivityLogsByProjectIdAsync(string projectId);
    }
}
