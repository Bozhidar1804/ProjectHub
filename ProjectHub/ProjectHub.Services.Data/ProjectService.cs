using Microsoft.EntityFrameworkCore;
using System.Globalization;

using ProjectHub.Data;
using ProjectHub.Data.Models;
using ProjectHub.Services.Data.Interfaces;
using ProjectHub.Web.ViewModels.Project;
using static ProjectHub.Common.GeneralApplicationConstants;

namespace ProjectHub.Services.Data
{
    public class ProjectService : BaseService, IProjectService
    {
        private readonly ProjectHubDbContext dbContext;

        public ProjectService(ProjectHubDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<ProjectIndexViewModel>> GetAllProjectsAsync()
        {
            IEnumerable<ProjectIndexViewModel> projects = await this.dbContext
                .Projects
                .Where(p => p.IsDeleted == false)
                .Select(p => new ProjectIndexViewModel()
                {
                    Id = p.Id.ToString(),
                    CreatorId = p.CreatorId.ToString(),
                    Name = p.Name,
                    Description = p.Description,
                    StartDate = p.StartDate.ToString(DateFormat),
                    EndDate = p.EndDate.ToString(DateFormat),
                    Status = p.Status,
                    TeamMembers = p.TeamMembers,
                    Candidatures = p.Candidatures
                })
                .ToListAsync();

            return projects;
        }

        public async Task<IEnumerable<ProjectIndexViewModel>> GetCreatorAllProjectsAsync(string userId)
        {
            IEnumerable<ProjectIndexViewModel> projects = await this.dbContext
                .Projects
                .Where(p => p.IsDeleted == false && p.CreatorId.ToString() == userId)
                .Select(p => new ProjectIndexViewModel()
                {
                    Id = p.Id.ToString(),
                    CreatorId = p.CreatorId.ToString(),
                    Name = p.Name,
                    Description = p.Description,
                    StartDate = p.StartDate.ToString(DateFormat),
                    EndDate = p.EndDate.ToString(DateFormat),
                    Status = p.Status,
                    TeamMembers = p.TeamMembers
                })
                .ToListAsync();

            return projects;
        }

        public async Task<bool> CreateProjectAsync(ProjectCreateFormModel model, string userId)
        {
            Guid userGuid = Guid.Empty;
            bool isUserGuidValid = IsGuidValid(userId, ref userGuid);

            if (!isUserGuidValid)
            {
                return false;
            }

            bool isStartDateValid = DateTime.TryParseExact(model.StartDate, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None,
                out DateTime releaseDate);

            bool isEndDateValid = DateTime.TryParseExact(model.EndDate, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None,
                out DateTime endDate);

            if (!isStartDateValid || !isEndDateValid)
            {
                return false;
            }

			ApplicationUser? creatorUser = await this.dbContext.Users.FindAsync(userGuid);
			if (creatorUser == null)
			{
				return false;
			}

			Project projectToAdd = new Project()
            {
                Name = model.Name,
                Description = model.Description,
                StartDate = releaseDate,
                EndDate = endDate,
                CreatorId = userGuid,
                Status = 0
            };

            projectToAdd.TeamMembers.Add(creatorUser);

            await this.dbContext.AddAsync(projectToAdd);
            await this.dbContext.SaveChangesAsync();

            return true;
        }

		public async Task<Project> GetProjectByIdAsync(string projectId)
		{
            Guid projectGuid = Guid.Empty;
            bool isProjectGuidValid = IsGuidValid(projectId, ref projectGuid);

			Project? project = await this.dbContext.Projects
                .Include(p => p.TeamMembers)
                .Include(p => p.Milestones)
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == projectGuid && !p.IsDeleted);

            if (project == null)
            {
				throw new KeyNotFoundException($"Project with ID {projectId} not found.");
			}

            return project;
		}

        public async Task<bool> SoftDeleteProjectAsync(string projectId)
        {
            Guid projectGuid = Guid.Empty;
            bool isProjectGuidValid = IsGuidValid(projectId, ref projectGuid);

            if (isProjectGuidValid == false)
            {
                return false;
            }

            Project? projectToDelete = await this.dbContext.Projects
                .FirstOrDefaultAsync(p => p.Id == projectGuid && !p.IsDeleted);

            if (projectToDelete == null)
            {
                return false;
            }

            projectToDelete.IsDeleted = true;
            await this.dbContext.SaveChangesAsync();

            return true;
        }

        public async System.Threading.Tasks.Task UpdateProjectAsync(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project), "Project cannot be null.");
            }

            Project? projectToUpdate = await this.dbContext.Projects
                .Include(p => p.TeamMembers)
                .FirstOrDefaultAsync(p => p.Id == project.Id);

            if (projectToUpdate == null)
            {
                throw new InvalidOperationException($"Project with ID {project.Id} not found.");
            }

            projectToUpdate.Name = project.Name;
            projectToUpdate.Description = project.Description;
            projectToUpdate.StartDate = project.StartDate;
            projectToUpdate.EndDate = project.EndDate;
            projectToUpdate.Status = project.Status;
            projectToUpdate.IsDeleted = project.IsDeleted;
            projectToUpdate.TeamMembers = project.TeamMembers;

            await this.dbContext.SaveChangesAsync();
        }

        public async Task<bool> SetMaxMilestonesAsync(string projectId, int maxMilestones)
        {
            Guid projectGuid = Guid.Empty;
            bool isProjectGuidValid = IsGuidValid(projectId, ref projectGuid);

            if (isProjectGuidValid == false)
            {
                return false;
            }

            Project? projectToUpdate = await this.dbContext.Projects
                .FirstOrDefaultAsync(p => p.Id == projectGuid && !p.IsDeleted);

            if (projectToUpdate == null)
            {
                throw new InvalidOperationException("Project not found.");
            }

            if (maxMilestones <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxMilestones), "Maximum milestones must be greater than zero.");
            }

            if (projectToUpdate.MaxMilestones.HasValue)
            {
                throw new InvalidOperationException("Maximum milestones for this project are already set.");
            }

            projectToUpdate.MaxMilestones = maxMilestones;
            await this.dbContext.SaveChangesAsync();

            return true;
        }
    }
}
