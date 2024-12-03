using System.ComponentModel.DataAnnotations;

using static ProjectHub.Common.EntityValidationConstants.Milestone;
using static ProjectHub.Common.NotificationMessagesConstants;

namespace ProjectHub.Web.ViewModels.Milestone
{
    public class MilestoneCreateInputModel
    {
        [Required]
        [StringLength(MilestoneNameMaxLength, MinimumLength = MilestoneNameMinLength, ErrorMessage = GeneralErrorMessage)]
        public string Name { get; set; } = null!;

        [Required]
        public string Deadline { get; set; } = null!;

        [Required]
        public string ProjectId { get; set; } = null!;
    }
}
