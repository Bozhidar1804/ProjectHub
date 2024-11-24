using Microsoft.EntityFrameworkCore;

using ProjectHub.Data;
using ProjectHub.Data.Models;
using ProjectHub.Data.Models.Enums;
using ProjectHub.Services.Data.Interfaces;
using ProjectHub.Web.ViewModels.Candidature;
using static ProjectHub.Common.GeneralApplicationConstants;

namespace ProjectHub.Services.Data
{
    public class CandidatureService : BaseService, ICandidatureService
    {
        private readonly ProjectHubDbContext dbContext;

        public CandidatureService(ProjectHubDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<CandidatureIndexViewModel>> GetAllCandidaturesAsync(string userId)
        {
            Guid userGuid = Guid.Empty;
            bool isUserGuidValid = IsGuidValid(userId, ref userGuid);

            IEnumerable<CandidatureIndexViewModel> allCandidatures = await this.dbContext
                .Candidatures
                .Where(c => c.ApplicantId == userGuid && c.IsDeleted == false)
                .Select(c => new CandidatureIndexViewModel()
                {
                    CandidatureId = c.Id.ToString(),
                    Content = c.Content,
                    DateApplied = c.ApplicationDate.ToString(DateFormat),
                    Status = c.Status,
                    ProjectName = c.Project.Name
                })
                .ToListAsync();

            return allCandidatures;
        }

        public async Task<bool> CreateCandidatureAsync(CandidatureCreateFormModel model, string userId)
        {
            Guid userGuid = Guid.Empty;
            bool isUserGuidValid = IsGuidValid(userId, ref userGuid);

            if (!isUserGuidValid)
            {
                return false;
            }

            string content = $"{model.Answer1}\n\n{model.Answer2}\n\n{model.Answer3}\n\n{model.Answer4}";

            ApplicationUser applicant = await this.dbContext.Users.FirstOrDefaultAsync(u => u.Id == userGuid);

            if (applicant == null)
            {
                return false;
            }

            Candidature candidatureToCreate = new Candidature()
            {
                ProjectId = model.ProjectId,
                ApplicantId = userGuid,
                Applicant = applicant,
                Content = content,
                Status = 0
            };

            await this.dbContext.AddAsync(candidatureToCreate);
            await this.dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<ICollection<Project>> GetAllModeratorProjectsByIdAsync(string moderatorId)
        {
            List<Project> moderatorProjects = await this.dbContext
                .Projects
                .Where(p => !p.IsDeleted && p.CreatorId.ToString() == moderatorId)
                .ToListAsync();

            return moderatorProjects;
        }

        public async Task<ICollection<Candidature>> GetCandidaturesForModeratorProjectsAsync(ICollection<Project> moderatorProjects)
        {
            ICollection<Candidature> candidaturesToReturn = await this.dbContext.Candidatures
                .Where(c => moderatorProjects.Select(p => p.Id).Contains(c.ProjectId)
                            && c.Status == CandidatureStatus.Pending
                            && !c.IsDeleted)
                .Include(c => c.Applicant)
                .ToListAsync();

            return candidaturesToReturn;
        }
    }
}
