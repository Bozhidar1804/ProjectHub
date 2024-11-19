using ProjectHub.Web.ViewModels.Project;

namespace ProjectHub.Services.Data.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectIndexViewModel>> GetAllProjectsAsync();

        Task<bool> CreateProjectAsync(ProjectCreateFormModel model, string userId);
    }
}
