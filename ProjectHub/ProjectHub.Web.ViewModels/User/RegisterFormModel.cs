using System.ComponentModel.DataAnnotations;

using static ProjectHub.Common.EntityValidationConstants.ApplicationUser;

namespace ProjectHub.Web.ViewModels.User
{
    public class RegisterFormModel
    {
        [Required]
        [StringLength(ApplicationUserFullNameMaxLength, MinimumLength = ApplicationUserFullNameMinLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")]
        [Display(Name = "FullName")]
        public string FullName { get; set; } = null!;

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(ApplicationUserPasswordMaxLength, MinimumLength = ApplicationUserPasswordMinLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
