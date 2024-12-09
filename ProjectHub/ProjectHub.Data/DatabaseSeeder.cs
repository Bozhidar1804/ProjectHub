using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using ProjectHub.Data.Models;
using ProjectHub.Data.Models.Enums;
using static ProjectHub.Common.GeneralApplicationConstants;

namespace ProjectHub.Data
{
    public static class DatabaseSeeder
    {
        public static readonly Guid AdminId = Guid.Parse("5F9E9D07-325F-46DB-8C91-2B5620F2113E");
        public static readonly Guid Moderator1Id = Guid.Parse("57CFBDE7-224C-4C9F-AEB4-8CFC5B4995A5");
        public static readonly Guid Moderator2Id = Guid.Parse("BE6CA69D-9036-4800-ADED-9D5ACFEEE304");
        public static readonly Guid User1Id = Guid.Parse("01A3B9F4-D0BF-4F28-8D69-37A8789D2E8E");
        public static readonly Guid User2Id = Guid.Parse("2EC1EA23-37CB-4333-8F37-409853C21627");
        public static readonly Guid User3Id = Guid.Parse("93D936A1-5C56-4209-BE28-7425C5998096");

        private static readonly Guid SoftwareDevProjectId = Guid.Parse("D1F10E0A-8EFA-4F1E-9C4C-7F5C6E3E4A98");
        private static readonly Guid ConstructionProjectId = Guid.Parse("A7B9F4D0-BF4F-28D6-9C4C-7F5C6E3E4A98");
        private static readonly Guid EventPlanningProjectId = Guid.Parse("B9A7F4D0-BF4F-28D6-9C4C-7F5C6E3E4A98");
        private static readonly Guid MarketingCampaignProjectId = Guid.Parse("D0A7B9F4-BF4F-28D6-9C4C-7F5C6E3E4A98");
        private static readonly Guid ResearchProjectId = Guid.Parse("F9A7B9F4-D0BF-4F28-8D69-37A8789D2E8E");

        public static void Seed(IServiceProvider serviceProvider)
        {
            using IServiceScope scope = serviceProvider.CreateScope();
            ProjectHubDbContext dbContext = scope.ServiceProvider.GetRequiredService<ProjectHubDbContext>();
            UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            RoleManager<IdentityRole<Guid>> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            // Seed methods for each entity
            SeedRoles(roleManager);
            SeedUsers(dbContext, userManager);
            SeedProjects(dbContext);
        }

        private static void SeedRoles(RoleManager<IdentityRole<Guid>> roleManager)
        {
            // Admin Role
            if (!roleManager.RoleExistsAsync(AdminRoleName).Result)
            {
                IdentityRole<Guid> adminRole = new IdentityRole<Guid>
                {
                    Name = AdminRoleName
                };
                roleManager.CreateAsync(adminRole).Wait();
            }

            // Moderator Role
            if (!roleManager.RoleExistsAsync(ModeratorRoleName).Result)
            {
                IdentityRole<Guid> moderatorRole = new IdentityRole<Guid>
                {
                    Name = ModeratorRoleName
                };
                roleManager.CreateAsync(moderatorRole).Wait();
            }

            // User Role
            if (!roleManager.RoleExistsAsync(UserRoleName).Result)
            {
                IdentityRole<Guid> userRole = new IdentityRole<Guid>
                {
                    Name = UserRoleName
                };
                roleManager.CreateAsync(userRole).Wait();
            }
        }

        private static void SeedUsers(ProjectHubDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            // Seed admin user
            if (!dbContext.Users.Any(u => u.Email == AdminEmail))
            {
                ApplicationUser admin = new ApplicationUser
                {
                    Id = AdminId,
                    FullName = "Admin User",
                    UserName = AdminEmail,
                    NormalizedUserName = AdminEmail.ToUpper(),
                    Email = AdminEmail,
                    NormalizedEmail = AdminEmail.ToUpper(),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    IsDeleted = false
                };
                string adminPass = "admin123";
                userManager.CreateAsync(admin, adminPass).Wait();

                // Admin role is assigned in a separate SeedAdministrator method
            }

            // Seed moderator users
            for (int i = 1; i <= 2; i++)
            {
                string email = $"moderator{i}@gmail.com";
                if (!dbContext.Users.Any(u => u.Email == email))
                {
                    ApplicationUser moderator = new ApplicationUser
                    {
                        Id = Guid.NewGuid(),
                        FullName = $"ProjectCreator {i}",
                        UserName = email,
                        NormalizedUserName = email.ToUpper(),
                        Email = email,
                        NormalizedEmail = email.ToUpper(),
                        SecurityStamp = Guid.NewGuid().ToString(),
                        IsDeleted = false
                    };

                    if (i == 1)
                    {
                        moderator.Id = Moderator1Id;
                    } else if (i == 2)
                    {
                        moderator.Id = Moderator2Id;
                    }

                    string moderatorPass = "mod123";
                    userManager.CreateAsync(moderator, moderatorPass).Wait();
                    userManager.AddToRoleAsync(moderator, ModeratorRoleName).Wait();
                }
            }

            // Seed basic users
            for (int i = 1; i <= 3; i++)
            {
                string email = $"user{i}@gmail.com";
                if (!dbContext.Users.Any(u => u.Email == email))
                {
                    ApplicationUser user = new ApplicationUser
                    {
                        Id = Guid.NewGuid(),
                        FullName = $"BasicUser {i}",
                        UserName = email,
                        NormalizedUserName = email.ToUpper(),
                        Email = email,
                        NormalizedEmail = email.ToUpper(),
                        SecurityStamp = Guid.NewGuid().ToString(),
                        IsDeleted = false
                    };

                    string userPass = "user123";
                    userManager.CreateAsync(user, userPass).Wait();
                    userManager.AddToRoleAsync(user, UserRoleName).Wait();
                }
            }

        }

        private static void SeedProjects(ProjectHubDbContext dbContext)
        {
            if (!dbContext.Projects.Any())
            {
                List<Project> projects = new List<Project>
                {
                    new Project
                    {
                        Id = SoftwareDevProjectId,
                        Name = "Software Development Project",
                        Description = "Developing a new web application for e-commerce.",
                        CreatorId = Moderator1Id,
                        Status = ProjectStatus.InProgress,
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddMonths(1),
                        MaxMilestones = 4
                    },
                    new Project
                    {
                        Id = ConstructionProjectId,
                        Name = "Construction Project",
                        Description = "Building a new office complex from scratch.",
                        CreatorId = Moderator2Id,
                        Status = ProjectStatus.InProgress,
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddMonths(2),
                        MaxMilestones = 3
                    },
                    new Project
                    {
                        Id = EventPlanningProjectId,
                        Name = "Event Planning",
                        Description = "Planning a charity ball with multiple factions and mystical games.",
                        CreatorId = Moderator1Id,
                        Status = ProjectStatus.InProgress,
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddMonths(3),
                        MaxMilestones = 4
                    },
                    new Project
                    {
                        Id = MarketingCampaignProjectId,
                        Name = "Marketing Campaign",
                        Description = "Launching a new product with a comprehensive marketing strategy.",
                        CreatorId = Moderator2Id,
                        Status = ProjectStatus.InProgress,
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddMonths(1),
                        MaxMilestones = 2
                    },
                    new Project
                    {
                        Id = ResearchProjectId,
                        Name = "Research Project",
                        Description = "Conducting market research to understand customer needs.",
                        CreatorId = Moderator1Id,
                        Status = ProjectStatus.InProgress,
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddMonths(1),
                        // No milestones, must be set from the corresponding view.
                    }
                };

                foreach (Project project in projects)
                {
                    ApplicationUser creator = dbContext.Users.FirstOrDefault(u => u.Id == project.CreatorId)!;

                    if (creator != null)
                    {
                        project.Creator = creator;

                        if (project.TeamMembers == null)
                        {
                            project.TeamMembers = new List<ApplicationUser>();
                        }

                        project.TeamMembers.Add(creator);
                    }
                }

                dbContext.Projects.AddRange(projects);
                dbContext.SaveChanges();
            }
        }
    }
}
