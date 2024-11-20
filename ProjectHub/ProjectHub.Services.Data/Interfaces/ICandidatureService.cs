using ProjectHub.Web.ViewModels.Candidature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectHub.Services.Data.Interfaces
{
    public interface ICandidatureService
    {
        Task<bool> CreateCandidatureAsync(CandidatureCreateFormModel model, string userId);
        Task<IEnumerable<CandidatureIndexViewModel>> GetAllCandidaturesAsync(string userId);
    }
}
