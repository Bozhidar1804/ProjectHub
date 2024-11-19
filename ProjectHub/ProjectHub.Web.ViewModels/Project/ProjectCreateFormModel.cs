using System.ComponentModel.DataAnnotations;

using static ProjectHub.Common.EntityValidationConstants.Project;
using static ProjectHub.Common.GeneralApplicationConstants;

namespace ProjectHub.Web.ViewModels.Project
{
    public class ProjectCreateFormModel
    {
        public ProjectCreateFormModel()
        {
            this.StartDate = DateTime.UtcNow.ToString(DateFormat);
        }

        [Required]
        [StringLength(ProjectNameMaxLength, MinimumLength = ProjectNameMinLength, ErrorMessage = "{0} must be between {1} and {2} characters.")]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(ProjectDescriptionMaxLength, ErrorMessage = "{0} cannot exceed {1} characters.")]
        public string Description { get; set; } = null!;

        [Required]
        public string StartDate { get; set; } = null!;

        [Required]
        public string EndDate { get; set; } = null!;
    }
}
