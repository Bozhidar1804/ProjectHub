using System.ComponentModel.DataAnnotations;

namespace ProjectHub.Data.Models.Enums
{
	public enum TaskAction
	{
		Created = 0,
		ReAssigned = 1,
        Completed = 2,
		[Display(Name = "Comment Added")]
		CommentAdded = 3
	}
}
