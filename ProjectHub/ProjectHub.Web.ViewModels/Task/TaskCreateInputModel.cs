using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

using ProjectHub.Data.Models.Enums;
using static ProjectHub.Common.EntityValidationConstants.Task;

namespace ProjectHub.Web.ViewModels.Task
{
    public class TaskCreateInputModel
    {
        [Required]
        [StringLength(TaskTitleMaxLength, MinimumLength = TaskTitleMinLength)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(TaskDescriptionMaxLength, MinimumLength = TaskDescriptionMinLength)]
        public string Description { get; set; } = null!;

        [Required]
        public string DueDate { get; set; } = null!;

        [Required]
        public TaskPriority Priority { get; set; }

        [Required]
        public string AssignedToUserId { get; set; } = null!;

        [Required]
        public string ProjectId { get; set; } = null!;

        [Required(ErrorMessage = "Please select a milestone.")]
        public string MilestoneId { get; set; } = null!;

        public IEnumerable<SelectListItem> Users { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Priorities { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Milestones { get; set; } = new List<SelectListItem>();
    }
}
