using ProjectHub.Web.ViewModels.Task;

namespace ProjectHub.Services.Data.Interfaces
{
    public interface ITaskService
    {
        Task<ProjectHub.Data.Models.Task> GetTaskByIdAsync(string taskId);
        Task<List<ProjectHub.Data.Models.Task>> GetTasksByProjectIdAsync(string projectId);
        Task<bool> CreateTaskAsync(TaskCreateFormModel model);

        Task<IEnumerable<TaskIndexViewModel>> GetTasksAssignedToUserAsync(string userId);
    }
}
