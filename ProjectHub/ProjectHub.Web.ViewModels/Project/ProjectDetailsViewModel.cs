using ProjectHub.Data.Models.Enums;
using ProjectHub.Web.ViewModels.Milestone;
using ProjectHub.Web.ViewModels.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectHub.Web.ViewModels.Project
{
    public class ProjectDetailsViewModel
    {
        public string ProjectId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string StartDate { get; set; } = null!;
        public string EndDate { get; set; } = null!;
        public ProjectStatus Status { get; set; }
        public string CreatorName { get; set; } = null!;
        public int TeamMemberCount { get; set; }
        public int MaxMilestones { get; set; }
        public List<ProjectMemberViewModel> Members { get; set; } = new List<ProjectMemberViewModel>();
        public List<MilestoneProjectDetailsViewModel> Milestones { get; set; } = new List<MilestoneProjectDetailsViewModel>();
        public List<TaskProjectDetailsViewModel> Tasks { get; set; } = new List<TaskProjectDetailsViewModel>();
    }
}
