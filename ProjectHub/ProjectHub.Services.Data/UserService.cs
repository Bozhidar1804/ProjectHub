using Microsoft.EntityFrameworkCore;
using ProjectHub.Data;
using ProjectHub.Data.Models;
using ProjectHub.Services.Data.Interfaces;

namespace ProjectHub.Services.Data
{
    public class UserService : IUserService
    {
        private readonly ProjectHubDbContext dbContext;

        public UserService(ProjectHubDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<string> GetFullNameByEmailAsync(string email)
        {
            ApplicationUser? user = await this.dbContext
                .Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return string.Empty;
            }

            return user.FullName;
        }
    }
}
