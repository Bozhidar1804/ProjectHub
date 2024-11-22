using ProjectHub.Data.Models;
using ProjectHub.Data.Models.Enums;

namespace ProjectHub.Web.ViewModels.Project
{
    public class ProjectIndexViewModel
    {
        public string Id { get; set; } = null!;
        public string CreatorId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string StartDate { get; set; } = null!;
        public string EndDate { get; set; } = null!;
        public ProjectStatus Status { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<ApplicationUser> TeamMembers { get; set; } = new List<ApplicationUser>();
    }
}
