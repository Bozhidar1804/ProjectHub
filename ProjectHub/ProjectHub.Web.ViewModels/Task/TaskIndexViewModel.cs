using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectHub.Web.ViewModels.Task
{
    public class TaskIndexViewModel
    {
        public string Id { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string DueDate { get; set; } = null!;
        public string ProjectName { get; set; } = null!;
        public string MilestoneName { get; set; } = null!;
        public string Priority { get; set; } = null!;
        public string ProjectId {  get; set; } = null!;
        public string MilestoneId { get; set; } = null!;
    }
}
