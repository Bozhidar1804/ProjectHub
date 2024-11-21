using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using ProjectHub.Data.Models;
using ProjectHub.Web.Areas.Admin.Services;
using ProjectHub.Web.Areas.Admin.Services.Interfaces;
using ProjectHub.Web.Areas.Admin.ViewModels;
using System.Data;
using static ProjectHub.Common.GeneralApplicationConstants;

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
        public async Task<IActionResult> UserRoles()
        {
            List<UserRoleViewModel> userRoles = await this.managementService.GetUserRolesAsync();
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

            return NotFound(); // Or handle a more specific error response
        }
    }
}
