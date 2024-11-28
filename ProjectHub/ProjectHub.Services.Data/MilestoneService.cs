using Microsoft.EntityFrameworkCore;

using ProjectHub.Data;
using ProjectHub.Data.Models;
using ProjectHub.Services.Data.Interfaces;

namespace ProjectHub.Services.Data
{
    public class MilestoneService : BaseService, IMilestoneService
    {
        private readonly ProjectHubDbContext dbContext;

        public MilestoneService(ProjectHubDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Milestone> GetMilestoneByIdAsync(string milestoneId)
        {
            Guid milestoneGuid = Guid.Empty;
            bool isMilestoneGuidValid = IsGuidValid(milestoneId, ref milestoneGuid);

            Milestone milestone = await this.dbContext.Milestones
                .FirstOrDefaultAsync(m => m.Id == milestoneGuid && !m.IsDeleted);

            if (milestone == null)
            {
                throw new KeyNotFoundException($"Project with ID {milestoneId} not found.");
            }

            return milestone;
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
