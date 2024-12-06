using Microsoft.AspNetCore.Identity;

using ProjectHub.Web.Areas.Admin.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

using ProjectHub.Web.Areas.Admin.ViewModels;
using ProjectHub.Data.Models;
using static ProjectHub.Common.GeneralApplicationConstants;
using ProjectHub.Data;
using ProjectHub.Data.Models.Enums;
using System;

namespace ProjectHub.Web.Areas.Admin.Services
{
    public class ManagementService : BaseAdminService, IManagementService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole<Guid>> roleManager;
        private readonly ProjectHubDbContext dbContext;

        public ManagementService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, ProjectHubDbContext dbContext)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.dbContext = dbContext;
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

        public async Task<IEnumerable<ProjectManagementViewModel>> GetAllProjectsAsync()
        {
            List<ProjectManagementViewModel> projectsToManage = await this.dbContext.Projects
            .Include(p => p.Creator)
            .Include(p => p.Tasks)
            .Include(p => p.Milestones)
            .Select(p => new ProjectManagementViewModel
            {
                ProjectId = p.Id.ToString(),
                Title = p.Name,
                CreatorName = p.Creator.UserName,
                CreatedOn = p.StartDate.ToString(DateFormat),
                IsDeleted = p.IsDeleted
            })
            .ToListAsync();

            return projectsToManage;
        }

        public async System.Threading.Tasks.Task SoftDeleteProjectAsync(string projectId)
        {
            Guid projectGuid = Guid.Empty;
            bool isProjectGuidValid = IsGuidValid(projectId, ref projectGuid);

            Project project = await this.dbContext.Projects.FirstOrDefaultAsync(p => p.Id == projectGuid && !p.IsDeleted);

            project.IsDeleted = true;
            await this.dbContext.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task RestoreProjectAsync(string projectId)
        {
            Guid projectGuid = Guid.Empty;
            bool isProjectGuidValid = IsGuidValid(projectId, ref projectGuid);

            Project project = await this.dbContext.Projects.FirstOrDefaultAsync(p => p.Id == projectGuid && p.IsDeleted);

            project.IsDeleted = false;
            await this.dbContext.SaveChangesAsync();
        }

        public async Task<StatisticsViewModel> GetStatisticsAsync()
        {
            int totalUsers = await this.dbContext.Users.CountAsync();

            int totalModerators = await this.dbContext.UserRoles
                .Where(ur => ur.RoleId.ToString() == "3023933A-87F2-431C-6C52-08DD0A822BEA")
                .CountAsync();

            int totalProjects = await this.dbContext.Projects
                .Where(p => !p.IsDeleted)
                .CountAsync();

            int totalCandidatures = await this.dbContext.Candidatures.CountAsync();

            int pendingCandidatures = await this.dbContext.Candidatures
                .Where(c => c.Status == CandidatureStatus.Pending)
                .CountAsync();

            int approvedCandidatures = await this.dbContext.Candidatures
                .Where(c => c.Status == CandidatureStatus.Approved)
                .CountAsync();

            int deniedCandidatures = await this.dbContext.Candidatures
                .Where(c => c.Status == CandidatureStatus.Denied)
                .CountAsync();

            int totalMilestones = await this.dbContext.Milestones
                .Where(m => !m.IsDeleted)
                .CountAsync();

            int totalTasks = await this.dbContext.Tasks
                .Where(t => !t.IsDeleted)
                .CountAsync();

            StatisticsViewModel model = new StatisticsViewModel()
            {
                TotalUsers = totalUsers,
                TotalModerators = totalModerators,
                TotalProjects = totalProjects,
                TotalCandidatures = totalCandidatures,
                PendingCandidatures = pendingCandidatures,
                ApprovedCandidatures = approvedCandidatures,
                DeniedCandidatures = deniedCandidatures,
                TotalMilestones = totalMilestones,
                TotalTasks = totalTasks,
            };

            return model;
        }
    }
}
