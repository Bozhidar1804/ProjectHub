using ProjectHub.Data.Models;

namespace ProjectHub.Services.Data.Interfaces
{
    public interface IMilestoneService
    {
       Task<Milestone> GetMilestoneByIdAsync(string milestoneId);
       Task<List<Milestone>> GetMilestonesByProjectIdAsync(string projectId);
    }
}
