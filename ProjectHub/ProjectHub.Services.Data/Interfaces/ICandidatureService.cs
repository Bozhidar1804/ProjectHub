using ProjectHub.Web.ViewModels.Candidature;
using ProjectHub.Data.Models;

namespace ProjectHub.Services.Data.Interfaces
{
    public interface ICandidatureService
    {
        Task<bool> CreateCandidatureAsync(CandidatureCreateFormModel model, string userId);
        Task<IEnumerable<CandidatureIndexViewModel>> GetAllCandidaturesAsync(string userId);
        Task<ICollection<Project>> GetAllModeratorProjectsByIdAsync(string moderatorId);

        Task<ICollection<Candidature>> GetCandidaturesForModeratorProjectsAsync(ICollection<Project> moderatorProjects);
        Task<Candidature> GetCandidatureByIdAsync(string candidatureId);
        System.Threading.Tasks.Task UpdateCandidatureAsync(Candidature candidature);
    }
}
