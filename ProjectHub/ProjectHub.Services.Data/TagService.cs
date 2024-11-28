using Microsoft.EntityFrameworkCore;

using ProjectHub.Data;
using ProjectHub.Data.Models;
using ProjectHub.Services.Data.Interfaces;

namespace ProjectHub.Services.Data
{
    public class TagService : BaseService, ITagService
    {
        private readonly ProjectHubDbContext dbContext;

        public TagService(ProjectHubDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Tag> GetTagByIdAsync(string tagId)
        {
            Guid tagGuid = Guid.Empty;
            bool isTagGuidValid = IsGuidValid(tagId, ref tagGuid);

            Tag tag = await this.dbContext.Tags
                .FirstOrDefaultAsync(t => t.Id == tagGuid && !t.IsDeleted);

            if (tag == null)
            {
                throw new KeyNotFoundException($"Project with ID {tagId} not found.");
            }

            return tag;
        }

        public async Task<List<string>> GetTagsByProjectIdAsync(string projectId)
        {
            Guid projectGuid = Guid.Empty;
            bool isProjectGuidValid = IsGuidValid(projectId, ref projectGuid);

            List<string> tags = await dbContext.Tasks
                .Where(t => t.ProjectId == projectGuid && !t.IsDeleted)
                .SelectMany(t => t.Tags)
                .Select(tag => tag.Name)
                .Distinct()
                .ToListAsync();

            return tags;
        }
    }
}
