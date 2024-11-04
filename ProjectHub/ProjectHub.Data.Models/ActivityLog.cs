using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProjectHub.Data.Models.Enums;
using static ProjectHub.Common.EntityValidationConstants.ActivityLog;

namespace ProjectHub.Data.Models
{
	public class ActivityLog
	{
		[Key]
		public Guid Id { get; set; } = Guid.NewGuid();

		[Required]
		[MaxLength(ActivityLogReasonMaxLength)]
		public string Reason { get; set; } = null!;
		public TaskAction Action { get; set; }
		public DateTime ActionDate { get; set; }
		public bool IsDeleted { get; set; }

		[Required]
		public Guid TaskId { get; set; }
		[ForeignKey(nameof(TaskId))]
		public Task Task { get; set; } = null!;

	}
}
