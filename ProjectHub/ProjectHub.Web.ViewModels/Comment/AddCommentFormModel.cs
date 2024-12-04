using System.ComponentModel.DataAnnotations;

using static ProjectHub.Common.EntityValidationConstants.Comment;
using static ProjectHub.Common.NotificationMessagesConstants;

namespace ProjectHub.Web.ViewModels.Comment
{
    public class AddCommentFormModel
    {
        [Required]
        [StringLength(CommentContentMaxLength, MinimumLength = CommentContentMinLength, ErrorMessage = GeneralErrorMessage)]
        public string Content { get; set; } = null!;

        public string TaskId { get; set; } = null!;
		public string TaskTitle { get; set; } = null!;
    }
}
