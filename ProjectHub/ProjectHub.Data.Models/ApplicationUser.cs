using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

using static ProjectHub.Common.EntityValidationConstants.TeamMember;


namespace ProjectHub.Data.Models
{
	public class ApplicationUser : IdentityUser<Guid>
	{
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid();
			this.Email = string.Empty;
        }

		[Required]
		[MaxLength(TeamMemberFullNameMaxLength)]
		public string FullName { get; set; } = null!;

		[Required]
		public string Role { get; set; } = null!;

		public bool IsDeleted { get; set; }

        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
