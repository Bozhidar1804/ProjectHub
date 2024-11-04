using System.ComponentModel.DataAnnotations;

using static ProjectHub.Common.EntityValidationConstants.TeamMember;


namespace ProjectHub.Data.Models
{
	public class TeamMember
	{
		[Key]
		public Guid Id { get; set; } = Guid.NewGuid();

		[Required]
		[MaxLength(TeamMemberFullNameMaxLength)]
		public string FullName { get; set; } = null!;

		[Required]
		[MaxLength(TeamMemberEmailMaxLength)]
		public string Email { get; set; } = null!;

		[Required]
		public string Role { get; set; } = null!;

		public bool IsDeleted { get; set; }
	}
}
