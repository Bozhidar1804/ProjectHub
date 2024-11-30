using System.ComponentModel.DataAnnotations;

using ProjectHub.Data.Models.Enums;
using static ProjectHub.Common.EntityValidationConstants.Project;

namespace ProjectHub.Data.Models
{
	public class Project
	{
		[Key]
		public Guid Id { get; set; } = Guid.NewGuid();

		[Required]
		[MaxLength(ProjectNameMaxLength)]
		public string Name { get; set; } = null!;

		[Required]
		[MaxLength(ProjectDescriptionMaxLength)]
		public string Description { get; set; } = null!;
		public int? MaxMilestones { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public ProjectStatus Status { get; set; }
		public bool IsDeleted { get; set; }

		[Required]
		public Guid CreatorId { get; set; }
		public ApplicationUser Creator { get; set; } = null!;

		public ICollection<Task> Tasks { get; set; } = new List<Task>();
		public ICollection<Milestone> Milestones { get; set; } = new List<Milestone>();
		public ICollection<ApplicationUser> TeamMembers { get; set; } = new List<ApplicationUser>();
		public ICollection<Candidature> Candidatures { get; set; } = new List<Candidature>();
	}
}
