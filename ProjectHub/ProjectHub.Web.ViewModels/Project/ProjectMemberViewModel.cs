namespace ProjectHub.Web.ViewModels.Project
{
    public class ProjectMemberViewModel
    {
        public string UserId { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Role { get; set; } = null!;
        public int CompletedTasks { get; set; }
    }
}
