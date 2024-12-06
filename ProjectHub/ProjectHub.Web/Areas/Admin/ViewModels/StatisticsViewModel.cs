namespace ProjectHub.Web.Areas.Admin.ViewModels
{
    public class StatisticsViewModel
    {
        public int TotalProjects { get; set; }
        public int TotalUsers { get; set; }
        public int TotalModerators { get; set; }
        public int TotalCandidatures { get; set; }
        public int PendingCandidatures { get; set; }
        public int ApprovedCandidatures { get; set; }
        public int DeniedCandidatures { get; set; }
        public int TotalMilestones {  get; set; }
        public int TotalTasks { get; set; }
    }
}
