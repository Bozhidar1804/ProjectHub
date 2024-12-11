using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using ProjectHub.Data.Models;
using ProjectHub.Web.Areas.Admin.Services;
using ProjectHub.Web.Areas.Admin.Services.Interfaces;
using ProjectHub.Web.Areas.Admin.ViewModels;
using ProjectHub.Web.Helpers;

namespace ProjectHub.Web.Areas.Admin.Controllers
{
    public class ManagementController : BaseAdminController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IManagementService managementService;

        public ManagementController(UserManager<ApplicationUser> userManager, IManagementService managementService)
        {
            this.userManager = userManager;
            this.managementService = managementService;
        }

        [HttpGet]
        public async Task<IActionResult> UserRoles(int pageIndex = 1)
        {
            const int pageSize = 3; // Customize the page size as needed
            PaginatedList<UserRoleViewModel> userRoles = await this.managementService.GetAllUsersWithRolesAsync(pageIndex, pageSize);
            return View(userRoles);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeUserRole(string userId, string roleName)
        {
            bool success = await managementService.ChangeUserRoleAsync(userId, roleName);

            if (success)
            {
                return RedirectToAction(nameof(UserRoles));
            }

            return NotFound(); // Could handle a more specific error response
        }

        [HttpGet]
        public async Task<IActionResult> Projects()
        {
            var projects = await managementService.GetAllProjectsAsync();
            return View(projects);
        }

        [HttpPost]
        public async Task<IActionResult> SoftDeleteProject(string projectId)
        {
            await this.managementService.SoftDeleteProjectAsync(projectId);
            return RedirectToAction(nameof(Projects));
        }

        [HttpPost]
        public async Task<IActionResult> RestoreProject(string projectId)
        {
            await this.managementService.RestoreProjectAsync(projectId);
            return RedirectToAction(nameof(Projects));
        }

        [HttpGet]
        public async Task<IActionResult> Statistics()
        {
            StatisticsViewModel statistics = await this.managementService.GetStatisticsAsync();
            return View(statistics);
        }

        [HttpGet]
        public async Task<IActionResult> ViewActivity()
        {
            try
            {
                var activityLogs = await this.managementService.GetAllActivityLogsAsync();
                return View(activityLogs);
            }
            catch (Exception ex)
            {
                // Handle error gracefully
                this.ModelState.AddModelError(string.Empty, "An error occurred while retrieving activity logs.");
                return View();
            }
        }
    }
}
