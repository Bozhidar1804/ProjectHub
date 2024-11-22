using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using ProjectHub.Data.Models;
using ProjectHub.Services.Data.Interfaces;
using ProjectHub.Web.Infrastructure.Extensions;
using ProjectHub.Web.ViewModels.Project;
using static ProjectHub.Web.Infrastructure.Extensions.ClaimsPrincipalExtensions;
using static ProjectHub.Common.GeneralApplicationConstants;

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
        [Authorize(Roles = ModeratorRoleName)]
        public async Task<IActionResult> MyProjects()
        {
            string userId = this.User.GetUserId()!;
            IEnumerable<ProjectIndexViewModel> myProjectsModels = await this.projectService.GetCreatorAllProjectsAsync(userId);

            return View(myProjectsModels);
        }

        [HttpGet]
        [Authorize(Roles = ModeratorRoleName)]
        public IActionResult Create()
        {
            ProjectCreateFormModel model = new ProjectCreateFormModel();

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = ModeratorRoleName)]
        public async Task<IActionResult> Create(ProjectCreateFormModel model)
        {
            if (!ModelState.IsValid)
            {
                this.ModelState.AddModelError(string.Empty, "An error occurred while creating the project!");
                return View(model);
            }

            string userId = ClaimsPrincipalExtensions.GetUserId(User)!;
            bool isAddedResult = await projectService.CreateProjectAsync(model, userId);

            if (!isAddedResult)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while creating the project.");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
		public async Task<IActionResult> Delete(string Id)
		{
			ProjectDeleteViewModel projectModel = await this.projectService.GetProjectByIdAsync(Id);

			if (projectModel == null || projectModel.IsDeleted)
			{
				return NotFound();
			}

			return View(projectModel);
		}

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string Id)
        {
            bool result = await this.projectService.SoftDeleteProjectAsync(Id);

            if (!result)
            {
                return BadRequest("Unable to delete the project. Please try again.");
            }

            return RedirectToAction(nameof(MyProjects));
        }

    }
}
