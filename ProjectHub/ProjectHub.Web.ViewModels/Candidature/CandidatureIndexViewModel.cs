using ProjectHub.Common;
using ProjectHub.Data.Models.Enums;

namespace ProjectHub.Web.ViewModels.Candidature
{
    public class CandidatureIndexViewModel
    {
        public string CandidatureId { get; set; } = null!;
        public string DateApplied { get; set; } = DateTime.UtcNow.ToString(GeneralApplicationConstants.DateFormat);
        public CandidatureStatus Status { get; set; }
        public string ProjectName { get; set; } = null!;
        public int AnswersWordCount { get; set; }
    }
}
