using System.ComponentModel.DataAnnotations;

using static ProjectHub.Common.EntityValidationConstants.Tag;

namespace ProjectHub.Data.Models
{
	public class Tag
	{
		[Key]
		public Guid Id { get; set; } = Guid.NewGuid();

		[Required]
		[MaxLength(TagNameMaxLength)]
		public string Name { get; set; } = null!;
		public bool IsDeleted { get; set; }

		public ICollection<Task> Tasks { get; set; } = new HashSet<Task>();
	}
}
