using ProjectHub.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectHub.Web.ViewModels.Candidature
{
    public class CandidatureDetailsViewModel
    {
        public string Id { get; set; } = null!;
        public List<CandidatureContentModel> Content { get; set; } = null!;
        public string ProjectName { get; set; } = null!;
        public string ApplicantName { get; set; } = null!;
        public CandidatureStatus Status { get; set; }
        public DateTime ApplicationDate { get; set; }
    }
}
