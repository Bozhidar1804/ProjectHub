using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using ProjectHub.Data.Models;
using ProjectHub.Web.ViewModels.User;

namespace ProjectHub.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserStore<ApplicationUser> userStore;

        public UserController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IUserStore<ApplicationUser> userStore)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.userStore = userStore;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ApplicationUser user = new ApplicationUser()
            {
                FullName = model.FullName
            };

            await this.userManager.SetEmailAsync(user, model.Email);
            await this.userManager.SetUserNameAsync(user, model.FullName);

            IdentityResult result = await this.userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    return View(error);
                }
            }

            await this.signInManager.SignInAsync(user, isPersistent: false);

            return RedirectToAction("Index", "Home");
        }
    }
}
