using System.Globalization;
using Microsoft.EntityFrameworkCore;

using ProjectHub.Data;
using ProjectHub.Data.Models;
using ProjectHub.Services.Data.Interfaces;
using ProjectHub.Web.ViewModels.Milestone;
using static ProjectHub.Common.GeneralApplicationConstants;

namespace ProjectHub.Services.Data
{
	public class MilestoneService : BaseService, IMilestoneService
    {
        private readonly ProjectHubDbContext dbContext;

        public MilestoneService(ProjectHubDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

		public async Task<bool> CreateMilestoneAsync(MilestoneCreateInputModel model)
		{
			Guid projectGuid = Guid.Empty;
			bool isProjectGuidValid = IsGuidValid(model.ProjectId, ref projectGuid);

			if (!isProjectGuidValid)
			{
				return false;
			}

			bool isDeadlineDateFormatValid = DateTime.TryParseExact(model.Deadline, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None,
				out DateTime deadline);

            if (!isDeadlineDateFormatValid)
            {
                return false;
            }

            Project? project = await this.dbContext.Projects
                 .Where(p => p.Id == projectGuid)
                 .FirstOrDefaultAsync();

            if (project == null)
            {
                return false;
            }

            if (deadline < project.StartDate || deadline > project.EndDate)
            {
                return false;
            }

            Milestone milestoneToAdd = new Milestone()
            {
                Name = model.Name,
                Deadline = deadline,
                ProjectId = projectGuid,
                Project = project
            };

            await this.dbContext.Milestones.AddAsync(milestoneToAdd);
            await this.dbContext.SaveChangesAsync();

            return true;
		}

        public async Task<List<Milestone>> GetMilestonesByProjectIdAsync(string projectId)
        {
            Guid projectGuid = Guid.Empty;
            bool isProjectGuidValid = IsGuidValid(projectId, ref projectGuid);

            List<Milestone> milestones = await dbContext.Milestones
                .Include(m => m.Project)
                .Where(m => m.ProjectId == projectGuid && !m.IsDeleted)
                .OrderBy(m => m.Deadline)
                .ToListAsync();

            return milestones;
        }
    }
}
