using ProjectHub.Data.Models;
using ProjectHub.Web.ViewModels.Milestone;

namespace ProjectHub.Services.Data.Interfaces
{
    public interface IMilestoneService
    {
        Task<List<Milestone>> GetMilestonesByProjectIdAsync(string projectId);
       Task<bool> CreateMilestoneAsync(MilestoneCreateInputModel model);
	}
}
