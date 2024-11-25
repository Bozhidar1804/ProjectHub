using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectHub.Web.ViewModels.Candidature
{
    public class CandidatureToReviewViewModel
    {
        public Guid Id { get; set; }
        public string ApplicantName { get; set; } = null!;
        public string ApplicantEmail { get; set; } = null!;
        public DateTime ApplicationDate { get; set; }
    }
}
