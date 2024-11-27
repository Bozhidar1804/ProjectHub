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
		public TaskAction Action { get; set; }
		public DateTime Timestamp { get; set; }
		public bool IsDeleted { get; set; }

		[Required]
		public Guid TaskId { get; set; }
		[ForeignKey(nameof(TaskId))]
		public Task Task { get; set; } = null!;

        [Required]
        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

    }
}
