using ProjectHub.Web.ViewModels.ActivityLog;
using ProjectHub.Web.ViewModels.Tag;

namespace ProjectHub.Web.ViewModels.Task
{
    public class TaskViewModel
    {
        public string Id { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string AssignedTo { get; set; } = null!;
        public string Priority { get; set; } = null!;
        public TagViewModel Tag { get; set; } = new TagViewModel();
        public List<ActivityLogViewModel> ActivityLogs { get; set; } = new List<ActivityLogViewModel>();
    }
}
