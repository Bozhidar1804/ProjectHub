using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectHub.Data.Models;

namespace ProjectHub.Data.Configuration
{
    public class TeamMemberConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasKey(tm => tm.Id);

            builder
                .HasMany(tm => tm.Projects)
                .WithMany(p => p.TeamMembers);

            builder
                .Property(tm => tm.IsDeleted)
                .HasDefaultValue(false);
        }
    }
}
