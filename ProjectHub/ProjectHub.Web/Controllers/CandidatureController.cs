using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using ProjectHub.Services.Data.Interfaces;
using ProjectHub.Data.Models;
using ProjectHub.Web.ViewModels.Candidature;
using ProjectHub.Web.Infrastructure.Extensions;
using static ProjectHub.Common.GeneralApplicationConstants;
using Newtonsoft.Json;

namespace ProjectHub.Web.Controllers
{
    [Authorize]
    public class CandidatureController : Controller
    {
        private readonly IProjectService projectService;
        private readonly ICandidatureService candidatureService;
        private readonly UserManager<ApplicationUser> userManager;

        public CandidatureController(IProjectService projectService, ICandidatureService candidatureService, UserManager<ApplicationUser> userManager)
        {
            this.projectService = projectService;
            this.candidatureService = candidatureService;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string userId = ClaimsPrincipalExtensions.GetUserId(User)!;
            IEnumerable<CandidatureIndexViewModel> candidatures = await this.candidatureService.GetAllCandidaturesAsync(userId);

            return View(candidatures);
        }

        [HttpGet]
        public IActionResult Create(Guid projectId)
        {
            CandidatureCreateFormModel model = new CandidatureCreateFormModel
            {
                ProjectId = projectId
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CandidatureCreateFormModel model)
        {
            if (!ModelState.IsValid)
            {
                this.ModelState.AddModelError(string.Empty, "An error occurred while creating the candidature!");
                return View(model);
            }

            string userId = ClaimsPrincipalExtensions.GetUserId(User)!;
            bool isAddedResult = await this.candidatureService.CreateCandidatureAsync(model, userId);

            if (!isAddedResult)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while creating the project.");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = ModeratorRoleName)]
        public async Task<IActionResult> ReviewAll()
        {
            string moderatorId = this.User.GetUserId();

            if (string.IsNullOrWhiteSpace(moderatorId))
            {
                return Unauthorized();
            }

            ICollection<Project> moderatorProjects = await this.candidatureService
                .GetAllModeratorProjectsByIdAsync(moderatorId);

            ICollection<Candidature> selectedCandidatures = await this.candidatureService
                .GetCandidaturesForModeratorProjectsAsync(moderatorProjects);

            List<CandidaturesGroupedByProjectViewModel> groupedCandidatures = selectedCandidatures
                .GroupBy(c => c.ProjectId)
                .Select(group => new CandidaturesGroupedByProjectViewModel
                {
                    ProjectId = group.Key,
                    ProjectName = moderatorProjects.First(p => p.Id == group.Key).Name,
                    Candidatures = group.Select(c => new CandidatureToReviewViewModel
                    {
                        Id = c.Id,
                        ApplicationDate = c.ApplicationDate,
                        ApplicantName = c.Applicant.FullName,
                        ApplicantEmail = c.Applicant.Email!
                    }).ToList()
                })
                .ToList();

            return View(groupedCandidatures);
        }

        [HttpGet]
        [Authorize(Roles = ModeratorRoleName)]
        public async Task<IActionResult> Decide(string candidatureId)
        {
            if (string.IsNullOrWhiteSpace(candidatureId))
            {
                return NotFound("Invalid candidature ID. Please try again.");
            }

            Candidature candidature = await this.candidatureService.GetCandidatureByIdAsync(candidatureId);

            if (candidature == null || candidature.IsDeleted)
            {
                return NotFound("Candidature not found or has been deleted.");
            }

            List<CandidatureContentModel> deserializedContent = JsonConvert.DeserializeObject<List<CandidatureContentModel>>(candidature.Content)!;

            CandidatureDecideViewModel candidatureDecideViewModel = new CandidatureDecideViewModel()
            {
                CandidatureId = candidature.Id.ToString(),
                ApplicantName = candidature.Applicant.FullName,
                ApplicationDate = candidature.ApplicationDate.ToString(DateFormat),
                Content = deserializedContent!,
                ProjectName = candidature.Project.Name ?? "Unknown Project"
            };

            return View(candidatureDecideViewModel);
        }

    }
}
