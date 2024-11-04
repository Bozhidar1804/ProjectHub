using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ProjectHub.Common.EntityValidationConstants.Comment;

namespace ProjectHub.Data.Models
{
	public class Comment
	{
		[Key]
		public Guid Id { get; set; } = Guid.NewGuid();

		[Required]
		[MaxLength(CommentContentMaxLength)]
		public string Content { get; set; } = null!;

		public DateTime CreatedOn { get; set; }
		public bool IsDeleted { get; set; }

		[Required]
		public Guid TaskId { get; set; }
		[ForeignKey(nameof(TaskId))]
		public Task Task { get; set; } = null!;
	}
}
