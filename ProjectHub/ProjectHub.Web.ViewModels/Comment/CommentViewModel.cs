namespace ProjectHub.Web.ViewModels.Comment
{
    public class CommentViewModel
    {
        public string CommentId { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string CreatedOn { get; set; } = null!;
        public string AuthorName { get; set; } = null!;
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
    }
}
