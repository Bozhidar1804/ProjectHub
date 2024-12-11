using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
                .HasOne(t => t.Milestone)
                .WithMany(m => m.Tasks)
                .HasForeignKey(t => t.MilestoneId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(t => t.IsCompleted)
            .HasDefaultValue(false);

            builder
                .Property(t => t.IsDeleted)
                .HasDefaultValue(false);
        }
    }
}
