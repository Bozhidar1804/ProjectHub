using ProjectHub.Data.Models.Enums;

namespace ProjectHub.Web.Areas.Admin.ViewModels
{
    public class ActivityLogViewModel
    {
        public string TaskTitle { get; set; } = string.Empty;
        public TaskAction Action { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Timestamp { get; set; } = string.Empty;
    }
}
