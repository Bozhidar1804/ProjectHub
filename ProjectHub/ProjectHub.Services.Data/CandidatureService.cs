using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
                    AnswersWordCount = GetAnswerWordCount(c.Content),
                    DateApplied = c.ApplicationDate.ToString(DateFormat),
                    Status = c.Status,
                    ProjectName = c.Project.Name
                })
                .ToListAsync();

            return allCandidatures;
        }

        public async Task<bool> CreateCandidatureAsync(CandidatureCreateInputModel model, string userId)
        {
            Guid userGuid = Guid.Empty;
            bool isUserGuidValid = IsGuidValid(userId, ref userGuid);

            if (!isUserGuidValid)
            {
                return false;
            }

            List<CandidatureContentModel> content = new List<CandidatureContentModel>
            {
                new CandidatureContentModel { Question = Question1, Answer = model.Answer1 },
                new CandidatureContentModel { Question = Question2, Answer = model.Answer2 },
                new CandidatureContentModel { Question = Question3, Answer = model.Answer3 },
                new CandidatureContentModel { Question = Question4, Answer = model.Answer4 },
            };
            string serializedContent = JsonConvert.SerializeObject(content);

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
                Content = serializedContent,
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

        public async Task<Candidature> GetCandidatureByIdAsync(string candidatureId)
        {
            Guid candidatureGuid = Guid.Empty;
            bool isCandidatureGuidValid = IsGuidValid(candidatureId, ref candidatureGuid);

            if (!isCandidatureGuidValid)
            {
                return null;
            }

            Candidature candidatureToReturn = await this.dbContext.Candidatures
                .Include(c => c.Applicant)
                .Include(c => c.Project)
                .FirstOrDefaultAsync(c => c.Id == candidatureGuid && !c.IsDeleted);

            if (candidatureToReturn == null)
            {
                throw new KeyNotFoundException($"Candidature with ID {candidatureId} not found.");
            }

            return candidatureToReturn;
        }

        private static int GetAnswerWordCount(string content)
        {
            var answers = JsonConvert.DeserializeObject<List<CandidatureContentModel>>(content);
            return answers?.Sum(a => a.Answer.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length) ?? 0;
        }

        public async System.Threading.Tasks.Task UpdateCandidatureAsync(Candidature candidature)
        {
            if (candidature == null)
            {
                throw new ArgumentNullException(nameof(candidature), "Candidature cannot be null.");
            }

            Candidature candidatureToUpdate = await this.dbContext.Candidatures
                .FirstOrDefaultAsync(c => c.Id == candidature.Id);

            if (candidatureToUpdate == null)
            {
                throw new InvalidOperationException($"Candidature with ID {candidature.Id} not found.");
            }

            candidatureToUpdate.Content = candidature.Content;
            candidatureToUpdate.ApplicationDate = candidature.ApplicationDate;
            candidatureToUpdate.Status = candidature.Status;
            candidatureToUpdate.ProjectId = candidature.ProjectId;
            candidatureToUpdate.ApplicantId = candidature.ApplicantId;

            await this.dbContext.SaveChangesAsync();
        }
    }
}
