using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public bool IsCompleted { get; set; }
        public bool IsDeleted { get; set; }


		[Required]
		public Guid ProjectId { get; set; }

		[ForeignKey(nameof(ProjectId))]
		public Project Project { get; set; } = null!;

        [Required]
        public Guid MilestoneId { get; set; }

        [ForeignKey(nameof(MilestoneId))]
        public Milestone Milestone { get; set; } = null!;

        [Required]
		public Guid AssignedToUserId { get; set; }
		[ForeignKey(nameof(AssignedToUserId))]
		public ApplicationUser AssignedToUser { get; set; } = null!;

		public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
		public ICollection<ActivityLog> ActivityLogs { get; set; } = new HashSet<ActivityLog>();

	}
}
