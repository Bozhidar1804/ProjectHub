using System.ComponentModel.DataAnnotations;

using static ProjectHub.Common.EntityValidationConstants.Milestone;

namespace ProjectHub.Data.Models
{
	public class Milestone
	{
		[Key]
		public Guid Id { get; set; } = Guid.NewGuid();

		[Required]
		[MaxLength(MilestoneNameMaxLength)]
		public string Name { get; set; } = null!;

		public DateTime Deadline { get; set; }
		public bool IsCompleted {  get; set; }
		public bool IsDeleted { get; set; }

		[Required]
		public Guid ProjectId { get; set; }
		public Project Project { get; set; } = null!;
	}
}
