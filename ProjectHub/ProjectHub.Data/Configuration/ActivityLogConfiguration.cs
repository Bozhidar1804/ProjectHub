﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ProjectHub.Data.Models;

namespace ProjectHub.Data.Configuration
{
    public class ActivityLogConfiguration : IEntityTypeConfiguration<ActivityLog>
    {
        public void Configure(EntityTypeBuilder<ActivityLog> builder)
        {
            builder
                .HasKey(a => a.Id);

            builder
                .Property(a => a.TaskId)
                .IsRequired();

            builder.Property(al => al.Timestamp)
                .IsRequired();

            builder
                .HasOne(a => a.Task)
                .WithMany(t => t.ActivityLogs)
                .HasForeignKey(a => a.TaskId);

            builder
                .HasOne(al => al.User)
                .WithMany()
                .HasForeignKey(al => al.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Property(tm => tm.IsDeleted)
                .HasDefaultValue(false);
        }
    }
}
