using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using ProjectHub.Services.Data.Interfaces;
using ProjectHub.Data.Models;
using ProjectHub.Web.ViewModels.Candidature;
using ProjectHub.Web.Infrastructure.Extensions;

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
    }
}
