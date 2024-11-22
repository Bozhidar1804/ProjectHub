using Microsoft.EntityFrameworkCore;


using ProjectHub.Data;
using ProjectHub.Data.Models;
using ProjectHub.Services.Data.Interfaces;
using ProjectHub.Web.ViewModels.Project;
using static ProjectHub.Common.GeneralApplicationConstants;
using System.Globalization;

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
                    Status = p.Status
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
                    Status = p.Status
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

            Project projectToAdd = new Project()
            {
                Name = model.Name,
                Description = model.Description,
                StartDate = releaseDate,
                EndDate = endDate,
                CreatorId = userGuid,
                Status = 0
            };

            await this.dbContext.AddAsync(projectToAdd);
            await this.dbContext.SaveChangesAsync();

            return true;
        }
    }
}
