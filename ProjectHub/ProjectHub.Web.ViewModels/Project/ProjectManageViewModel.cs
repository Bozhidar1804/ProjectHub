using ProjectHub.Web.ViewModels.Milestone;
using ProjectHub.Web.ViewModels.Task;

namespace ProjectHub.Web.ViewModels.Project
{
    public class ProjectManageViewModel
    {
        public string ProjectId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int? MaxMilestones { get; set; }
        public List<ProjectMemberViewModel> Members { get; set; } = new List<ProjectMemberViewModel>();
        public List<MilestoneViewModel> Milestones { get; set; } = new List<MilestoneViewModel>();
        public List<TaskViewModel> Tasks { get; set; } = new List<TaskViewModel>();
    }
}
