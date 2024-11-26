using ProjectHub.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectHub.Services.Data.Interfaces
{
    public interface IUserService
    {
        Task<string> GetFullNameByEmailAsync(string email);
        Task<ApplicationUser> GetUserByIdAsync(string userId);
    }
}
