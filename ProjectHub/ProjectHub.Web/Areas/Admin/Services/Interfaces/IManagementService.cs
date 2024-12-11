using ProjectHub.Web.Areas.Admin.ViewModels;
using ProjectHub.Web.Helpers;

namespace ProjectHub.Web.Areas.Admin.Services.Interfaces
{
    public interface IManagementService
    {
        Task<PaginatedList<UserRoleViewModel>> GetAllUsersWithRolesAsync(int pageIndex, int pageSize);
        Task<bool> ChangeUserRoleAsync(string userId, string role);
        Task<IEnumerable<ProjectManagementViewModel>> GetProjectsByFilterAsync(string filter);
        Task SoftDeleteProjectAsync(string projectId);
        Task RestoreProjectAsync(string projectId);
        Task<StatisticsViewModel> GetStatisticsAsync();
        Task<List<ActivityLogViewModel>> GetAllActivityLogsAsync();
    }
}
