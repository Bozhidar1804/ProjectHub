using System.ComponentModel.DataAnnotations;

using static ProjectHub.Common.EntityValidationConstants.ApplicationUser;

namespace ProjectHub.Web.ViewModels.User
{
    public class LoginFormModel
    {
        [Required]
        [StringLength(ApplicationUserFullNameMaxLength, MinimumLength = ApplicationUserFullNameMinLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")]
        public string Username { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
