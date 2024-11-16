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
                .Property(au => au.IsDeleted)
                .HasDefaultValue(false);
        }
    }
}
