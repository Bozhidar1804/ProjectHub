using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using ProjectHub.Services.Data.Interfaces;
using ProjectHub.Data.Models;
using ProjectHub.Web.ViewModels.Milestone;
using static ProjectHub.Common.GeneralApplicationConstants;
using static ProjectHub.Common.NotificationMessagesConstants;

namespace ProjectHub.Web.Controllers
{
    public class MilestoneController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IProjectService projectService;
        private readonly IMilestoneService milestoneService;

        public MilestoneController(UserManager<ApplicationUser> userManager, IProjectService projectService, IMilestoneService milestoneService)
        {
            this.userManager = userManager;
            this.projectService = projectService;
            this.milestoneService = milestoneService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = ModeratorRoleName)]
        public IActionResult Create(string projectId)
        {
			MilestoneCreateFormModel model = new MilestoneCreateFormModel
            {
                ProjectId = projectId
            };

            return View(model);
        }

		[HttpPost]
		[Authorize(Roles = ModeratorRoleName)]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(MilestoneCreateFormModel model)
        {
			if (!ModelState.IsValid)
			{
				this.ModelState.AddModelError(string.Empty, "An error occurred while creating the milestone!");
				return View(model);
			}

			bool isAddedResult = await milestoneService.CreateMilestoneAsync(model);

			if (!isAddedResult)
			{
				ModelState.AddModelError(string.Empty, "An error occurred while creating the milestone.");
				return View(model);
			}

            return RedirectToAction("Manage", "Project", new { projectId = model.ProjectId });
		}

	}
}
