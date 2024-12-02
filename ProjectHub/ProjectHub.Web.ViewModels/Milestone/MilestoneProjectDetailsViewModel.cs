using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectHub.Web.ViewModels.Milestone
{
    public class MilestoneProjectDetailsViewModel
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Deadline { get; set; } = null!;
        public bool IsCompleted { get; set; }
        public double Progress { get; set; }
    }
}
