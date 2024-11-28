using ProjectHub.Web.ViewModels.ActivityLog;
using ProjectHub.Web.ViewModels.Milestone;
using ProjectHub.Web.ViewModels.Tag;
using ProjectHub.Web.ViewModels.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectHub.Web.ViewModels.Project
{
    public class ProjectManageViewModel
    {
        public string ProjectId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public List<ProjectMemberViewModel> Members { get; set; } = new List<ProjectMemberViewModel>();
        public List<MilestoneViewModel> Milestones { get; set; } = new List<MilestoneViewModel>();
        public List<TaskViewModel> Tasks { get; set; } = new List<TaskViewModel>();
    }
}
