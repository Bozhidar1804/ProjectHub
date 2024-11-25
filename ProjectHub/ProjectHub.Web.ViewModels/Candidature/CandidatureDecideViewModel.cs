namespace ProjectHub.Web.ViewModels.Candidature
{
    public class CandidatureDecideViewModel
    {
        public string CandidatureId { get; set; } = null!;
        public string ApplicantName { get; set; } = null!;
        public List<CandidatureContentModel> Content { get; set; } = null!;
        public string ProjectName { get; set; } = null!;
        public string ApplicationDate { get; set; } = null!;
    }
}
