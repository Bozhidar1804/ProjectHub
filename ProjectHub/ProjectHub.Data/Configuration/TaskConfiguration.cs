﻿using Microsoft.EntityFrameworkCore;
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
                .HasMany(t => t.Tags)
                .WithMany(tag => tag.Tasks);

            builder
                .HasMany(t => t.Comments)
                .WithOne(c => c.Task)
                .HasForeignKey(c => c.TaskId);

            builder
                .HasMany(t => t.ActivityLogs)
                .WithOne(a => a.Task)
                .HasForeignKey(a => a.TaskId);

            builder
                .HasMany(t => t.Checklists)
                .WithOne(c => c.Task)
                .HasForeignKey(c => c.TaskId);

            builder
                .Property(t => t.IsDeleted)
                .HasDefaultValue(false);
        }
    }
}