using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProjectHub.Web.Data
{
	public class ProjectHubDbContext : IdentityDbContext
	{
		public ProjectHubDbContext(DbContextOptions<ProjectHubDbContext> options)
			: base(options)
		{
		}
	}
}
