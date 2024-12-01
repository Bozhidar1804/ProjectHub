using System.ComponentModel.DataAnnotations;

using static ProjectHub.Common.EntityValidationConstants.ApplicationUser;

namespace ProjectHub.Web.ViewModels.User
{
    public class LoginInputModel
    {
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
