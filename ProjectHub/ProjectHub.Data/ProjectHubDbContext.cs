using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using ProjectHub.Data.Configuration;
using ProjectHub.Data.Models;

namespace ProjectHub.Data
{
	public class ProjectHubDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public ProjectHubDbContext()
        {
            
        }

        public ProjectHubDbContext(DbContextOptions<ProjectHubDbContext> options)
			: base(options)
		{
		}

		public DbSet<Project> Projects { get; set; }
		public DbSet<Models.Task> Tasks { get; set; }
		public DbSet<User> TeamMembers { get; set; }
		public DbSet<Milestone> Milestones { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<ActivityLog> ActivityLogs { get; set; }
		public DbSet<CheckList> CheckLists { get; set; }
		public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ProjectConfiguration());
            modelBuilder.ApplyConfiguration(new TaskConfiguration());
            modelBuilder.ApplyConfiguration(new TeamMemberConfiguration());
            modelBuilder.ApplyConfiguration(new ChecklistConfiguration());
            modelBuilder.ApplyConfiguration(new MilestoneConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new ActivityLogConfiguration());
            modelBuilder.ApplyConfiguration(new TagConfiguration());
        }
    }
}
