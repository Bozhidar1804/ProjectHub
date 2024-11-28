using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectHub.Web.ViewModels.ActivityLog
{
    public class ActivityLogViewModel
    {
        public string Id { get; set; } = null!;
        public string Action { get; set; } = null!;
        public string PerformedBy { get; set; } = null!;
        public string Timestamp { get; set; } = null!;
    }
}
