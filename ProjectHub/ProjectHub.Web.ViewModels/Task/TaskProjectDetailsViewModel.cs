using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectHub.Web.ViewModels.Task
{
    public class TaskProjectDetailsViewModel
    {
        public string Id { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string AssignedTo { get; set; } = null!;
        public string MilestoneName { get; set; } = null!;
        public string Priority { get; set; } = null!;
        public bool IsCompleted { get; set; }
    }
}
