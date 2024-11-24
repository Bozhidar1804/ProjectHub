using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectHub.Web.ViewModels.Candidature
{
    public class CandidaturesGroupedByProjectViewModel
    {
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; } = null!;
        public List<CandidatureToReviewViewModel> Candidatures { get; set; } = new List<CandidatureToReviewViewModel>();
    }
}
