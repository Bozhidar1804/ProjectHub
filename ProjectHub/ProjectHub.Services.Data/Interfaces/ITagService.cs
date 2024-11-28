using ProjectHub.Data.Models;

namespace ProjectHub.Services.Data.Interfaces
{
    public interface ITagService
    {
        Task<Tag> GetTagByIdAsync(string tagId);
        Task<List<string>> GetTagsByProjectIdAsync(string projectId);
    }
}
