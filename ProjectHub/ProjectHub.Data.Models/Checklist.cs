using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ProjectHub.Common.EntityValidationConstants.Checklist;

namespace ProjectHub.Data.Models
{
	public class CheckList
	{
		[Key]
		public Guid Id { get; set; } = Guid.NewGuid();

		[Required]
		[MaxLength(ChecklistContentMaxLength)]
		public string Content { get; set; } = null!;
		public bool IsComplete { get; set; }
		public bool IsDeleted { get; set; }

		[Required]
		public Guid TaskId { get; set; }
		[ForeignKey(nameof(TaskId))]
		public Task? Task { get; set; }
	}
}
