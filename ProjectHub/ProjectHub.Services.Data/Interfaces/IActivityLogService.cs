using ProjectHub.Data.Models;
using ProjectHub.Data.Models.Enums;

namespace ProjectHub.Services.Data.Interfaces
{
    public interface IActivityLogService
    {
        System.Threading.Tasks.Task LogActionAsync(TaskAction action, string taskId, string userId);
    }
}
