using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectHub.Data.Models;

namespace ProjectHub.Data.Configuration
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder
                .Property(au => au.FullName)
                .HasDefaultValue("DefaultFullName");

            builder
                .HasKey(au => au.Id);

            builder
                .HasMany(au => au.Projects)
                .WithMany(p => p.TeamMembers);

            builder
                .HasMany(au => au.Tasks)
                .WithOne(t => t.AssignedToUser)
                .HasForeignKey(t => t.AssignedToUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(au => au.ActivityLogs)
                .WithOne(al => al.User)
                .HasForeignKey(al => al.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Property(au => au.IsDeleted)
                .HasDefaultValue(false);
        }
    }
}
