using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectHub.Web.ViewModels.Candidature
{
    public class CandidatureCreateFormModel
    {
        public Guid ProjectId { get; set; }
        public string Answer1 { get; set; } = null!; // What interests you about this project?
        public string Answer2 { get; set; } = null!; // What skills can you contribute?
        public string Answer3 { get; set; } = null!; // Why should we choose you?
        public string Answer4 { get; set; } = null!; // What are your goals for joining this project?
    }
}
