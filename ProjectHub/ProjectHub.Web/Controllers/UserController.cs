using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using ProjectHub.Data.Models;
using ProjectHub.Web.ViewModels.User;
using static ProjectHub.Common.NotificationMessagesConstants.User;

namespace ProjectHub.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

        public UserController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
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

        [HttpGet]
        public async Task<IActionResult> Login(string? returnUrl = null)
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            LoginFormModel model = new LoginFormModel()
            {
                ReturnUrl = returnUrl
            };

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await this.signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                this.TempData[ErrorMessage] = "There was an error while logging you in. Please try again!";

                return View(model);
            }

            return Redirect(model.ReturnUrl ?? "/Home/Index");
        }
    }
}
