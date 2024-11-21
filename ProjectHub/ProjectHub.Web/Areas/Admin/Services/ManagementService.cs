using Microsoft.AspNetCore.Identity;

using ProjectHub.Web.Areas.Admin.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

using ProjectHub.Web.Areas.Admin.ViewModels;
using ProjectHub.Data.Models;
using static ProjectHub.Common.GeneralApplicationConstants;

namespace ProjectHub.Web.Areas.Admin.Services
{
    public class ManagementService : IManagementService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole<Guid>> roleManager;

        public ManagementService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<List<UserRoleViewModel>> GetUserRolesAsync()
        {
            List<ApplicationUser> users = await this.userManager.Users.ToListAsync();
            List<UserRoleViewModel> userRoles = new List<UserRoleViewModel>();

            foreach (ApplicationUser user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                userRoles.Add(new UserRoleViewModel()
                {
                    UserId = user.Id.ToString(),
                    UserName = user.UserName!,
                    Email = user.Email!,
                    Role = roles.FirstOrDefault() ?? "User"
                });
            }

            userRoles = userRoles
                .OrderBy(u => u.Role == AdminRoleName ? 1 :
                            u.Role == ModeratorRoleName ? 2 : 3) // Role-based sorting
                .ToList();

            return userRoles;
        }

        public async Task<bool> ChangeUserRoleAsync(string userId, string role)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false; // User not found
            }

            // If role is provided, perform role assignment
            if (!string.IsNullOrWhiteSpace(role))
            {
                // Ensure the role exists
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var newRole = new IdentityRole<Guid>(role);
                    await roleManager.CreateAsync(newRole);
                }

                // If user is not already in the target role, add them
                if (!await userManager.IsInRoleAsync(user, role))
                {
                    await userManager.AddToRoleAsync(user, role);
                }

                // Remove the old roles (if necessary)
                var currentRoles = await userManager.GetRolesAsync(user);
                foreach (var currentRole in currentRoles)
                {
                    if (currentRole != role) // Don't remove the newly added role
                    {
                        await userManager.RemoveFromRoleAsync(user, currentRole);
                    }
                }
            }
            else
            {
                // Demote to "User" if the user is a Moderator
                if (await userManager.IsInRoleAsync(user, ModeratorRoleName))
                {
                    await userManager.RemoveFromRoleAsync(user, ModeratorRoleName);

                    // Ensure "User" role exists
                    if (!await roleManager.RoleExistsAsync(UserRoleName))
                    {
                        var userRole = new IdentityRole<Guid>(UserRoleName);
                        await roleManager.CreateAsync(userRole);
                    }

                    // Add user to "User" role if not already
                    if (!await userManager.IsInRoleAsync(user, UserRoleName))
                    {
                        await userManager.AddToRoleAsync(user, UserRoleName);
                    }
                }
                else
                {
                    return false;  // User is not a Moderator, so no demotion is possible
                }
            }

            return true; // Successfully changed the role
        }
    }
}
