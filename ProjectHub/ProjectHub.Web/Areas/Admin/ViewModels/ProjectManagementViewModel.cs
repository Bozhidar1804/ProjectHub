namespace ProjectHub.Web.Areas.Admin.ViewModels
{
    public class ProjectManagementViewModel
    {
        public string ProjectId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string CreatorName { get; set; } = null!;
        public string CreatedOn { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public bool IsFlagged { get; set; }
    }
}
