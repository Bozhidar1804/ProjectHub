using System.ComponentModel.DataAnnotations;

using static ProjectHub.Common.EntityValidationConstants.Milestone;

namespace ProjectHub.Web.ViewModels.Milestone
{
    public class MilestoneCreateFormModel
    {
        [Required]
        [StringLength(MilestoneNameMaxLength, MinimumLength = MilestoneNameMinLength)]
        public string Name { get; set; } = null!;

        [Required]
        public string Deadline { get; set; } = null!;

        [Required]
        public string ProjectId { get; set; } = null!;
    }
}
