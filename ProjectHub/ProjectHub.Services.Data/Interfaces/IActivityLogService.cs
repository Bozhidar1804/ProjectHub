using ProjectHub.Data.Models;
using ProjectHub.Data.Models.Enums;

namespace ProjectHub.Services.Data.Interfaces
{
    public interface IActivityLogService
    {
        Task<ActivityLog> GetActivityLogByIdAsync(string activityLogId);

        Task<List<ActivityLog>> GetActivityLogsByProjectIdAsync(string projectId);
        System.Threading.Tasks.Task LogActionAsync(TaskAction action, string taskId, string userId);
    }
}
