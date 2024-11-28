namespace ProjectHub.Services.Data.Interfaces
{
    public interface ITaskService
    {
        Task<ProjectHub.Data.Models.Task> GetTaskByIdAsync(string taskId);
        Task<List<ProjectHub.Data.Models.Task>> GetTasksByProjectIdAsync(string projectId);
    }
}
