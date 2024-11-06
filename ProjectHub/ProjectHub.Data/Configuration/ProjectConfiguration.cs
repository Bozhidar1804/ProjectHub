﻿
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ProjectHub.Data.Models;

namespace ProjectHub.Data.Configuration
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder
                .HasKey(p => p.Id);

            builder
                .HasMany(p => p.Tasks)
                .WithOne(t => t.Project)
                .HasForeignKey(t => t.ProjectId);

            builder
                .HasMany(p => p.Milestones)
                .WithOne(m => m.Project)
                .HasForeignKey(m => m.ProjectId);

            builder
                .HasMany(p => p.TeamMembers)
                .WithMany(tm => tm.Projects);

            builder
                .Property(p => p.IsDeleted)
                .HasDefaultValue(false);
        }
    }
}