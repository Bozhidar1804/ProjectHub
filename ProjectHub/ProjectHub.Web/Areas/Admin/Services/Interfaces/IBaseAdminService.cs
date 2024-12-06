namespace ProjectHub.Web.Areas.Admin.Services.Interfaces
{
    public interface IBaseAdminService
    {
        bool IsGuidValid(string? id, ref Guid parsedGuid);
    }
}
