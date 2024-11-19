using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using ProjectHub.Data.Models;
using ProjectHub.Services.Data.Interfaces;
using ProjectHub.Web.Infrastructure.Extensions;
using ProjectHub.Web.ViewModels.Project;
using static ProjectHub.Web.Infrastructure.Extensions.ClaimsPrincipalExtensions;

namespace ProjectHub.Web.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IProjectService projectService;

        public ProjectController(UserManager<ApplicationUser> userManager, IProjectService projectService)
        {
            this.userManager = userManager;
            this.projectService = projectService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<ProjectIndexViewModel> projectsModels = await this.projectService.GetAllProjectsAsync();

            return View(projectsModels);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ProjectCreateFormModel model = new ProjectCreateFormModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProjectCreateFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string userId = ClaimsPrincipalExtensions.GetUserId(User)!;
            bool addedResult = await projectService.CreateProjectAsync(model, userId);

            if (!addedResult)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while creating the project.");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
