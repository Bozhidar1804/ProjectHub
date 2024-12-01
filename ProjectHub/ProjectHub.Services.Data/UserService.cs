using Microsoft.EntityFrameworkCore;
using ProjectHub.Data;
using ProjectHub.Data.Models;
using ProjectHub.Services.Data.Interfaces;

namespace ProjectHub.Services.Data
{
    public class UserService : BaseService, IUserService
    {
        private readonly ProjectHubDbContext dbContext;

        public UserService(ProjectHubDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            List<ApplicationUser> users = await this.dbContext.Users.ToListAsync();
            return users;
        }

        public async Task<string> GetFullNameByEmailAsync(string email)
        {
            ApplicationUser? user = await this.dbContext
                .Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return "User not found";
            }

            return user.FullName;
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            Guid userGuid = Guid.Empty;
            bool isUserGuidValid = IsGuidValid(userId, ref userGuid);

            ApplicationUser user = await this.dbContext
                .Users
                .FirstOrDefaultAsync(u => u.Id == userGuid);

            return user;
        }
    }
}
