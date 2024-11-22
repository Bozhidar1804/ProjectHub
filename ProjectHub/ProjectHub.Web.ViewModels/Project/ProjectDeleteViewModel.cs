using ProjectHub.Data.Models.Enums;

namespace ProjectHub.Web.ViewModels.Project
{
    public class ProjectDeleteViewModel
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public ProjectStatus Status { get; set; }
        public string EndDate { get; set; } = null!;
        public bool IsDeleted { get; set; }
    }
}
