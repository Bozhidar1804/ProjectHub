using ProjectHub.Web.ViewModels.ActivityLog;

namespace ProjectHub.Web.ViewModels.Task
{
    public class TaskViewModel
    {
        public string Id { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string AssignedTo { get; set; } = null!;
        public string Priority { get; set; } = null!;
        public List<ActivityLogViewModel> ActivityLogs { get; set; } = new List<ActivityLogViewModel>();
    }
}
