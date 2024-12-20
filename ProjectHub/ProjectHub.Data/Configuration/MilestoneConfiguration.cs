﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ProjectHub.Data.Models;

namespace ProjectHub.Data.Configuration
{
    public class MilestoneConfiguration : IEntityTypeConfiguration<Milestone>
    {
        public void Configure(EntityTypeBuilder<Milestone> builder)
        {
            builder
                .HasKey(m => m.Id);

            builder
                .Property(m => m.ProjectId)
                .IsRequired();

            builder
                .HasMany(p => p.Tasks)
                .WithOne(t => t.Milestone)
                .HasForeignKey(t => t.MilestoneId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(m => m.Project)
                .WithMany(p => p.Milestones)
                .HasForeignKey(m => m.ProjectId);

            builder
                .Property(m => m.IsCompleted)
                .HasDefaultValue(false);

            builder
                .Property(m => m.IsDeleted)
                .HasDefaultValue(false);
        }
    }
}
