using ProjectHub.Web.ViewModels.Comment;

namespace ProjectHub.Web.ViewModels.Task
{
    public class TaskCommentsViewModel
    {
        public string TaskId { get; set; } = null!;
        public string TaskTitle { get; set; } = null!;
        public List<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();
    }
}
