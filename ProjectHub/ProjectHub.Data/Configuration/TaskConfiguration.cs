using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ProjectHub.Data.Models;

namespace ProjectHub.Data.Configuration
{
    public class TaskConfiguration : IEntityTypeConfiguration<Models.Task>
    {
        public void Configure(EntityTypeBuilder<Models.Task> builder)
        {
            builder
                .HasKey(t => t.Id);

            builder
                .HasMany(t => t.Comments)
                .WithOne(c => c.Task)
                .HasForeignKey(c => c.TaskId);

            builder
                .HasMany(t => t.ActivityLogs)
                .WithOne(a => a.Task)
                .HasForeignKey(a => a.TaskId);

            builder
                .HasOne(t => t.AssignedToUser)
                .WithMany(au => au.Tasks)
                .HasForeignKey(t => t.AssignedToUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Property(t => t.IsDeleted)
                .HasDefaultValue(false);
        }
    }
}
