using System.ComponentModel.DataAnnotations;

using ProjectHub.Data.Models.Enums;
using static ProjectHub.Common.EntityValidationConstants.Task;

namespace ProjectHub.Data.Models
{
	public class Task
	{
		[Key]
		public Guid Id { get; set; } = Guid.NewGuid();

		[Required]
		[MaxLength(TaskTitleMaxLength)]
		public string Title { get; set; } = null!;

		[Required]
		[MaxLength(TaskDescriptionMaxLength)]
		public string Description { get; set; } = null!;
		public DateTime DueDate { get; set; }
		public TaskPriority Priority { get; set; }

	}
}
