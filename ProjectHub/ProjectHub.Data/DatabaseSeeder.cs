using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

using ProjectHub.Data.Models;
using ProjectHub.Data.Models.Enums;
using ProjectHub.Web.ViewModels.Candidature;
using static ProjectHub.Common.GeneralApplicationConstants;

namespace ProjectHub.Data
{
    public static class DatabaseSeeder
    {
        // Users
        public static readonly Guid AdminId = Guid.Parse("5F9E9D07-325F-46DB-8C91-2B5620F2113E");
        public static readonly Guid Moderator1Id = Guid.Parse("57CFBDE7-224C-4C9F-AEB4-8CFC5B4995A5");
        public static readonly Guid Moderator2Id = Guid.Parse("BE6CA69D-9036-4800-ADED-9D5ACFEEE304");
        public static readonly Guid User1Id = Guid.Parse("01A3B9F4-D0BF-4F28-8D69-37A8789D2E8E");
        public static readonly Guid User2Id = Guid.Parse("2EC1EA23-37CB-4333-8F37-409853C21627");
        public static readonly Guid User3Id = Guid.Parse("93D936A1-5C56-4209-BE28-7425C5998096");

        // Projects
        private static readonly Guid SoftwareDevProjectId = Guid.Parse("D1F10E0A-8EFA-4F1E-9C4C-7F5C6E3E4A98"); // Mod1
        private static readonly Guid ConstructionProjectId = Guid.Parse("A7B9F4D0-BF4F-28D6-9C4C-7F5C6E3E4A98"); // Mod2
        private static readonly Guid EventPlanningProjectId = Guid.Parse("B9A7F4D0-BF4F-28D6-9C4C-7F5C6E3E4A98"); // Mod1
        private static readonly Guid MarketingCampaignProjectId = Guid.Parse("D0A7B9F4-BF4F-28D6-9C4C-7F5C6E3E4A98"); // Mod2
        private static readonly Guid ResearchProjectId = Guid.Parse("F9A7B9F4-D0BF-4F28-8D69-37A8789D2E8E"); // Mod1

        // Candidatures
        private static readonly Guid Candidature1SoftDevProjId = Guid.Parse("1A2B3C4D-5E6F-7890-1234-56789ABCDE01");
        private static readonly Guid Candidature2ConstrProjId = Guid.Parse("2B3C4D5E-6F78-9012-3456-789ABCDE0123");
        private static readonly Guid Candidature3EventPlanProjId = Guid.Parse("3C4D5E6F-7890-1234-5678-9ABCDE012345");
        private static readonly Guid Candidature4MarkCampProjId = Guid.Parse("4D5E6F78-9012-3456-789A-BCDE01234567");
        private static readonly Guid Candidature5ResearchProjId = Guid.Parse("5E6F7890-1234-5678-9ABC-DE0123456789");
        private static readonly Guid Candidature6SoftDevProjId = Guid.Parse("6F789012-3456-789A-BCDE-0123456789AB");

        // Candidature Questions
        public const string Question1 = "1. What interests you about this project?";
        public const string Question2 = "2. What skills can you contribute?";
        public const string Question3 = "3. Why should we choose you?";
        public const string Question4 = "4. What are your goals for joining this project?";

        // SoftwareDevProject Milestones
        private static readonly Guid Milestone1Id = Guid.Parse("A1F10E0A-8EFA-4F1E-9C4C-7F5C6E3E4A01");
        private static readonly Guid Milestone2Id = Guid.Parse("B2F10E0A-8EFA-4F1E-9C4C-7F5C6E3E4A02");
        private static readonly Guid Milestone3Id = Guid.Parse("C3F10E0A-8EFA-4F1E-9C4C-7F5C6E3E4A03");
        private static readonly Guid Milestone4Id = Guid.Parse("D4F10E0A-8EFA-4F1E-9C4C-7F5C6E3E4A04");

        // Task for SoftwareDevProject
        private static readonly Guid Task1Id = Guid.Parse("E1D10E0A-8EFA-4F1E-9C4C-7F5C6E3E4A11"); // Milestone 1
        private static readonly Guid Task2Id = Guid.Parse("F2D10E0A-8EFA-4F1E-9C4C-7F5C6E3E4A22"); // Milestone 1

        private static readonly Guid Task3Id = Guid.Parse("03D10E0A-8EFA-4F1E-9C4C-7F5C6E3E4A33"); // Milestone 2
        private static readonly Guid Task4Id = Guid.Parse("8114B178-C622-4500-BF31-9F55073AE302"); // Milestone 2

        private static readonly Guid Task5Id = Guid.Parse("DC3C2585-BFA1-4F34-A392-CB23754D587A"); // Milestone 3
        private static readonly Guid Task6Id = Guid.Parse("D5D6E09C-3FE6-4778-842F-07D08F52A33C"); // Milestone 3

        private static readonly Guid Task7Id = Guid.Parse("AE9D55B3-EB3C-4B48-A5CD-F37BDFDF0BD9"); // Milestone 4
        private static readonly Guid Task8Id = Guid.Parse("748B8C07-D309-4034-8364-E9F06393C785"); // Milestone 4

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
            SeedCandidatures(dbContext);
            SeedMilestones(dbContext);
            SeedTasksAndActivityLogs(dbContext); // Throws an error at dbContext.SaveChanges(); for duplicate key (even though there aren't any duplicate keys, I have checked)
                                                 //, but seeds the entities despite the error.
                                                 // Just run the project again after it throws the error.
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
                    }
                    else if (i == 2)
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

                    if (i == 1)
                    {
                        user.Id = User1Id;
                    }
                    else if (i == 2)
                    {
                        user.Id = User2Id;
                    }
                    else if (i == 3)
                    {
                        user.Id = User3Id;
                    }

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
                DateTime now = DateTime.UtcNow;
                List<Project> projects = new List<Project>
                {
                    new Project
                    {
                        Id = SoftwareDevProjectId,
                        Name = "Software Development Project",
                        Description = "Developing a new web application for e-commerce.",
                        CreatorId = Moderator1Id,
                        Status = ProjectStatus.InProgress,
                        StartDate = now.AddMonths(-1),
                        EndDate = now.AddMonths(1),
                        MaxMilestones = 4
                    },
                    new Project
                    {
                        Id = ConstructionProjectId,
                        Name = "Construction Project",
                        Description = "Building a new office complex from scratch.",
                        CreatorId = Moderator2Id,
                        Status = ProjectStatus.InProgress,
                        StartDate = now.AddMonths(-1),
                        EndDate = now.AddMonths(2),
                        MaxMilestones = 3
                    },
                    new Project
                    {
                        Id = EventPlanningProjectId,
                        Name = "Event Planning",
                        Description = "Planning a charity ball with multiple factions and mystical games.",
                        CreatorId = Moderator1Id,
                        Status = ProjectStatus.InProgress,
                        StartDate = now.AddMonths(-1),
                        EndDate = now.AddMonths(3),
                        MaxMilestones = 4
                    },
                    new Project
                    {
                        Id = MarketingCampaignProjectId,
                        Name = "Marketing Campaign",
                        Description = "Launching a new product with a comprehensive marketing strategy.",
                        CreatorId = Moderator2Id,
                        Status = ProjectStatus.InProgress,
                        StartDate = now.AddMonths(-1),
                        EndDate = now.AddMonths(1),
                        MaxMilestones = 2
                    },
                    new Project
                    {
                        Id = ResearchProjectId,
                        Name = "Research Project",
                        Description = "Conducting market research to understand customer needs.",
                        CreatorId = Moderator1Id,
                        Status = ProjectStatus.InProgress,
                        StartDate = now.AddMonths(-1),
                        EndDate = now.AddMonths(1)
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

        private static void SeedCandidatures(ProjectHubDbContext dbContext)
        {
            if (!dbContext.Candidatures.Any())
            {
                DateTime now = DateTime.UtcNow;
                List<Candidature> candidatures = new List<Candidature>
                {
                    new Candidature
                    {
                        Id = Candidature1SoftDevProjId,
                        Content = JsonConvert.SerializeObject(new List<CandidatureContentModel>
                        {
                            new CandidatureContentModel { Question = Question1, Answer = "I find this project aligns with my passion for environmental conservation." },
                            new CandidatureContentModel { Question = Question2, Answer = "I have strong analytical and organizational skills." },
                            new CandidatureContentModel { Question = Question3, Answer = "My past experiences in similar projects make me a valuable team player." },
                            new CandidatureContentModel { Question = Question4, Answer = "To enhance my expertise and contribute to a meaningful cause." }
                        }),
                        ProjectId = SoftwareDevProjectId,
                        ApplicantId = User1Id,
                        Status = CandidatureStatus.Approved,
                        ApplicationDate = now.AddMonths(-1).AddDays(10) // 10 days after project start
                    },
                    new Candidature
                    {
                        Id = Candidature2ConstrProjId,
                        Content = JsonConvert.SerializeObject(new List<CandidatureContentModel>
                        {
                            new CandidatureContentModel { Question = Question1, Answer = "The innovative approach of this project fascinates me." },
                            new CandidatureContentModel { Question = Question2, Answer = "I am proficient in UI/UX design and front-end development." },
                            new CandidatureContentModel { Question = Question3, Answer = "My unique design perspective will enhance the project's user interface." },
                            new CandidatureContentModel { Question = Question4, Answer = "To develop my skills while contributing creatively." }
                        }),
                        ProjectId = ConstructionProjectId,
                        ApplicantId = User1Id,
                        Status = CandidatureStatus.Pending,
                        ApplicationDate = now.AddMonths(-1).AddDays(5), // 5 days after project start
                    },
                    new Candidature
                    {
                        Id = Candidature3EventPlanProjId,
                        Content = JsonConvert.SerializeObject(new List<CandidatureContentModel>
                        {
                            new CandidatureContentModel { Question = Question1, Answer = "I am inspired by the team's mission and dedication." },
                            new CandidatureContentModel { Question = Question2, Answer = "I excel at project management and deadline adherence." },
                            new CandidatureContentModel { Question = Question3, Answer = "My experience managing cross-functional teams will be beneficial." },
                            new CandidatureContentModel { Question = Question4, Answer = "To expand my leadership skills and help this project succeed." }
                        }),
                        ProjectId = EventPlanningProjectId,
                        ApplicantId = User2Id,
                        Status = CandidatureStatus.Approved,
                        ApplicationDate = now.AddMonths(-1).AddDays(15), // 15 days after project start
                    },
                    new Candidature
                    {
                        Id = Candidature4MarkCampProjId,
                        Content = JsonConvert.SerializeObject(new List<CandidatureContentModel>
                        {
                            new CandidatureContentModel { Question = Question1, Answer = "I admire the technical challenges this project offers." },
                            new CandidatureContentModel { Question = Question2, Answer = "I bring expertise in backend development and database management." },
                            new CandidatureContentModel { Question = Question3, Answer = "My technical problem-solving skills make me a strong candidate." },
                            new CandidatureContentModel { Question = Question4, Answer = "To grow my technical expertise in a collaborative environment." }
                        }),
                        ProjectId = MarketingCampaignProjectId,
                        ApplicantId = User2Id,
                        Status = CandidatureStatus.Pending,
                        ApplicationDate = now.AddMonths(-1).AddDays(3), // 3 days after project start
                    },
                    new Candidature
                    {
                        Id = Candidature5ResearchProjId,
                        Content = JsonConvert.SerializeObject(new List<CandidatureContentModel>
                        {
                            new CandidatureContentModel { Question = Question1, Answer = "The opportunity to work on a cutting-edge project excites me." },
                            new CandidatureContentModel { Question = Question2, Answer = "I have extensive experience in data analysis and visualization." },
                            new CandidatureContentModel { Question = Question3, Answer = "My ability to derive actionable insights from data will help the project." },
                            new CandidatureContentModel { Question = Question4, Answer = "To learn advanced techniques while contributing to a breakthrough initiative." }
                        }),
                        ProjectId = ResearchProjectId,
                        ApplicantId = User3Id,
                        Status = CandidatureStatus.Approved,
                        ApplicationDate = now.AddMonths(-1).AddDays(20), // 20 days after project start
                    },
                    new Candidature
                    {
                        Id = Candidature6SoftDevProjId,
                        Content = JsonConvert.SerializeObject(new List<CandidatureContentModel>
                        {
                            new CandidatureContentModel { Question = Question1, Answer = "This project aligns with my vision of creating impactful solutions." },
                            new CandidatureContentModel { Question = Question2, Answer = "I have experience in Agile methodologies and team coordination." },
                            new CandidatureContentModel { Question = Question3, Answer = "I can streamline workflows and ensure timely deliveries." },
                            new CandidatureContentModel { Question = Question4, Answer = "To implement efficient systems and grow as a professional." }
                        }),
                        ProjectId = SoftwareDevProjectId,
                        ApplicantId = User3Id,
                        Status = CandidatureStatus.Denied,
                        ApplicationDate = now.AddMonths(-1).AddDays(10), // 10 days after project start
                    }
                };

                foreach (Candidature candidature in candidatures)
                {
                    // Assign navigation properties
                    candidature.Project = dbContext.Projects.FirstOrDefault(p => p.Id == candidature.ProjectId)!;
                    candidature.Applicant = dbContext.Users.FirstOrDefault(u => u.Id == candidature.ApplicantId)!;

                    // If approved, add the applicant to the project's team members
                    if (candidature.Status == CandidatureStatus.Approved)
                    {
                        Project project = candidature.Project;
                        if (project.TeamMembers == null)
                        {
                            project.TeamMembers = new List<ApplicationUser>();
                        }

                        if (!project.TeamMembers.Any(tm => tm.Id == candidature.ApplicantId))
                        {
                            project.TeamMembers.Add(candidature.Applicant);
                        }
                    }
                }

                dbContext.Candidatures.AddRange(candidatures);
                dbContext.SaveChanges();
            }
        }

        public static void SeedMilestones(ProjectHubDbContext dbContext)
        {
            if (!dbContext.Milestones.Any())
            {
                List<Milestone> milestones = new List<Milestone>
                {
                    new Milestone
                    {
                        Id = Milestone1Id,
                        Name = "Requirements Gathering",
                        Deadline = DateTime.UtcNow.AddDays(7),
                        IsCompleted = false,
                        IsDeleted = false,
                        ProjectId = SoftwareDevProjectId
                    },
                    new Milestone
                    {
                        Id = Milestone2Id,
                        Name = "System Design",
                        Deadline = DateTime.UtcNow.AddDays(14),
                        IsCompleted = false,
                        IsDeleted = false,
                        ProjectId = SoftwareDevProjectId
                    },
                    new Milestone
                    {
                        Id = Milestone3Id,
                        Name = "Development Phase",
                        Deadline = DateTime.UtcNow.AddDays(21),
                        IsCompleted = false,
                        IsDeleted = false,
                        ProjectId = SoftwareDevProjectId
                    },
                    new Milestone
                    {
                        Id = Milestone4Id,
                        Name = "Final Testing and Deployment",
                        Deadline = DateTime.UtcNow.AddDays(28),
                        IsCompleted = false,
                        IsDeleted = false,
                        ProjectId = SoftwareDevProjectId
                    }
                };

                Project? project = dbContext.Projects.FirstOrDefault(p => p.Id == SoftwareDevProjectId);

                if (project != null)
                {
                    foreach (Milestone milestone in milestones)
                    {
                        if (milestone.ProjectId == project.Id)
                        {
                            milestone.Project = project;
                        }
                    }
                }

                dbContext.Milestones.AddRange(milestones);
                dbContext.SaveChanges();
            }
        }

        public static void SeedTasksAndActivityLogs(ProjectHubDbContext dbContext)
        {
            if (!dbContext.Tasks.Any())
            {
                List<Models.Task> tasks = new List<Models.Task>()
                {
                    // Milestone 1: Requirements Gathering
                    new Models.Task
                    {
                        Id = Task1Id,
                        Title = "Identify Stakeholder Needs",
                        Description = "Gather detailed requirements from all stakeholders.",
                        DueDate = DateTime.UtcNow.AddDays(5),
                        Priority = TaskPriority.Medium,
                        IsCompleted = false,
                        IsDeleted = false,
                        ProjectId = SoftwareDevProjectId,
                        MilestoneId = Milestone1Id,
                        AssignedToUserId = User1Id
                    },
                    new Models.Task
                    {
                        Id = Task2Id,
                        Title = "Document Requirements",
                        Description = "Compile and organize requirements into a formal document.",
                        DueDate = DateTime.UtcNow.AddDays(6),
                        Priority = TaskPriority.High,
                        IsCompleted = false,
                        IsDeleted = false,
                        ProjectId = SoftwareDevProjectId,
                        MilestoneId = Milestone1Id,
                        AssignedToUserId = Moderator1Id
                    },

                    // Milestone 2: System Design
                    new Models.Task
                    {
                        Id = Task3Id,
                        Title = "Create Architectural Diagram",
                        Description = "Design system architecture and prepare diagrams.",
                        DueDate = DateTime.UtcNow.AddDays(12),
                        Priority = TaskPriority.High,
                        IsCompleted = false,
                        IsDeleted = false,
                        ProjectId = SoftwareDevProjectId,
                        MilestoneId = Milestone2Id,
                        AssignedToUserId = Moderator1Id
                    },
                    new Models.Task
                    {
                        Id = Task4Id,
                        Title = "Define Data Models",
                        Description = "Establish database schema and relationships.",
                        DueDate = DateTime.UtcNow.AddDays(13),
                        Priority = TaskPriority.Medium,
                        IsCompleted = false,
                        IsDeleted = false,
                        ProjectId = SoftwareDevProjectId,
                        MilestoneId = Milestone2Id,
                        AssignedToUserId = User1Id
                    },

                    // Milestone 3: Development Phase
                    new Models.Task
                    {
                        Id = Task5Id,
                        Title = "Develop Core Modules",
                        Description = "Implement core functionalities of the application.",
                        DueDate = DateTime.UtcNow.AddDays(18),
                        Priority = TaskPriority.High,
                        IsCompleted = false,
                        IsDeleted = false,
                        ProjectId = SoftwareDevProjectId,
                        MilestoneId = Milestone3Id,
                        AssignedToUserId = User1Id
                    },
                    new Models.Task
                    {
                        Id = Task6Id,
                        Title = "Implement Authentication",
                        Description = "Develop user authentication and authorization.",
                        DueDate = DateTime.UtcNow.AddDays(19),
                        Priority = TaskPriority.Medium,
                        IsCompleted = false,
                        IsDeleted = false,
                        ProjectId = SoftwareDevProjectId,
                        MilestoneId = Milestone3Id,
                        AssignedToUserId = User1Id
                    },

                    // Milestone 4: Final Testing and Deployment
                    new Models.Task
                    {
                        Id = Task7Id,
                        Title = "Conduct User Testing",
                        Description = "Perform testing sessions with end-users.",
                        DueDate = DateTime.UtcNow.AddDays(25),
                        Priority = TaskPriority.High,
                        IsCompleted = false,
                        IsDeleted = false,
                        ProjectId = SoftwareDevProjectId,
                        MilestoneId = Milestone4Id,
                        AssignedToUserId = User1Id
                    },
                    new Models.Task
                    {
                        Id = Task8Id,
                        Title = "Deploy to Production",
                        Description = "Deploy the final application to the production environment.",
                        DueDate = DateTime.UtcNow.AddDays(27),
                        Priority = TaskPriority.High,
                        IsCompleted = false,
                        IsDeleted = false,
                        ProjectId = SoftwareDevProjectId,
                        MilestoneId = Milestone4Id,
                        AssignedToUserId = User1Id
                    }
                };

                Project? project = dbContext.Projects.FirstOrDefault(p => p.Id == Guid.Parse("D1F10E0A-8EFA-4F1E-9C4C-7F5C6E3E4A98"));
                Dictionary<Guid, Milestone> milestones = dbContext.Milestones.ToDictionary(m => m.Id);
                Dictionary<Guid, ApplicationUser> users = dbContext.Users.ToDictionary(u => u.Id);

                if (project != null && milestones != null && users != null)
                {
                    foreach (var task in tasks)
                    {
                        if (milestones.TryGetValue(task.MilestoneId, out var milestone) &&
                            users.TryGetValue(task.AssignedToUserId, out var user))
                        {
                            task.Milestone = milestone;
                            task.Project = project;
                            task.AssignedToUser = user;

                            // Register activity log for task creation
                            dbContext.ActivityLogs.Add(new ActivityLog
                            {
                                Action = TaskAction.Created,
                                Timestamp = DateTime.UtcNow,
                                TaskId = task.Id,
                                UserId = user.Id
                            });
                        }
                    }

                    dbContext.Tasks.AddRange(tasks);
                    dbContext.SaveChanges();
                }

                dbContext.Tasks.AddRange(tasks);
                dbContext.SaveChanges();
            }
        }
    }
}