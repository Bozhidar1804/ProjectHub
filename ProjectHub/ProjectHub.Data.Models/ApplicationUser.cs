using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

using static ProjectHub.Common.EntityValidationConstants.ApplicationUser;


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
		[MaxLength(ApplicationUserFullNameMaxLength)]
		public string FullName { get; set; } = null!;

		public bool IsDeleted { get; set; }

        public ICollection<Project> Projects { get; set; } = new List<Project>();
		public ICollection<Task> Tasks { get; set; } = new List<Task>();
        public ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();
        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
    }
}
