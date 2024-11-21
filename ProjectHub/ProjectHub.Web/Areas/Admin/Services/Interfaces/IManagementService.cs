using ProjectHub.Web.Areas.Admin.ViewModels;

namespace ProjectHub.Web.Areas.Admin.Services.Interfaces
{
    public interface IManagementService
    {
        Task<List<UserRoleViewModel>> GetUserRolesAsync();
        Task<bool> ChangeUserRoleAsync(string userId, string role);
    }
}
