using ProjectHub.Web.Areas.Admin.Services.Interfaces;

namespace ProjectHub.Web.Areas.Admin.Services
{
    public class BaseAdminService : IBaseAdminService
    {
        public bool IsGuidValid(string? id, ref Guid parsedGuid)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return false;
            }

            bool isGuidValid = Guid.TryParse(id, out parsedGuid);
            if (!isGuidValid)
            {
                return false;
            }

            return true;
        }
    }
}
