using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc.Rendering;

using static ProjectHub.Common.EntityValidationConstants.Task;
using static ProjectHub.Common.NotificationMessagesConstants;

namespace ProjectHub.Web.ViewModels.Task
{
    public class TaskEditFormModel
    {
        [Required]
        public string TaskId { get; set; } = null!;
        [Required]
        [StringLength(TaskTitleMaxLength, MinimumLength = TaskTitleMinLength, ErrorMessage = GeneralErrorMessage)]
        public string Title { get; set; } = null!;
        [Required]
        public string AssignedToUserId { get; set; } = null!;
        public List<SelectListItem> AvailableUsers { get; set; } = new List<SelectListItem>();
    }
}
