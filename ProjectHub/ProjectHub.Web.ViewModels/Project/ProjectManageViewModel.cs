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
        /*public List<MilestoneViewModel> Milestones { get; set; }
        public List<TaskViewModel> Tasks { get; set; }
        public List<TagViewModel> Tags { get; set; }
        public List<ActivityLogViewModel> ActivityLogs { get; set; }*/
    }
}
