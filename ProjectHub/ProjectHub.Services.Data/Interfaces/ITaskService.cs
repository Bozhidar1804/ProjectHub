﻿using ProjectHub.Web.ViewModels.Project;
using ProjectHub.Web.ViewModels.Task;

namespace ProjectHub.Services.Data.Interfaces
{
    public interface ITaskService
    {
        Task<ProjectHub.Data.Models.Task> GetTaskByIdAsync(string taskId);
        Task<List<ProjectHub.Data.Models.Task>> GetTasksByProjectIdAsync(string projectId);
        Task<TaskCreateResult> CreateTaskAsync(TaskCreateInputModel model);

        Task<IEnumerable<IEnumerable<IGrouping<string, TaskIndexViewModel>>>> GetGroupedTasksAssignedToUserAsync(string userId);

        Task<bool> CompleteTaskAsync(ProjectHub.Data.Models.Task task);
        Task<List<TaskCompletedViewModel>> GetCompletedTasksByUserAsync(string userId);
        Task<List<TaskEditFormModel>> RePopulateTasksWithAvailableUsersAsync(ProjectEditFormModel model);
        Task<string> GetTaskTitleAsync(string taskId);
    }
}
