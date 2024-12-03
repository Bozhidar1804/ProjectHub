using ProjectHub.Web.ViewModels.Task;
using System.ComponentModel.DataAnnotations;

using static ProjectHub.Common.EntityValidationConstants.Project;
using static ProjectHub.Common.NotificationMessagesConstants;

namespace ProjectHub.Web.ViewModels.Project
{
    public class ProjectEditFormModel
    {
        [Required]
        public string ProjectId { get; set; } = null!;

        [Required]
        [StringLength(ProjectNameMaxLength, MinimumLength = ProjectNameMinLength, ErrorMessage = GeneralErrorMessage)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(ProjectDescriptionMaxLength, MinimumLength = ProjectDescriptionMinLength, ErrorMessage = GeneralErrorMessage)]
        public string Description { get; set; } = null!;

        [Required]
        public string EndDate { get; set; } = null!;
        public int? MaxMilestones { get; set; }

        public List<TaskEditFormModel> Tasks { get; set; } = new List<TaskEditFormModel>();
    }
}
