using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProjectHub.Data
{
	public class ProjectHubDbContext : IdentityDbContext
	{
		public ProjectHubDbContext(DbContextOptions<ProjectHubDbContext> options)
			: base(options)
		{
		}
	}
}
