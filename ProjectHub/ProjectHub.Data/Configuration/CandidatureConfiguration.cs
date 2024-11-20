using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProjectHub.Data.Models.Enums;
using ProjectHub.Data.Models;
using static ProjectHub.Common.EntityValidationConstants.Candidature;

namespace ProjectHub.Data.Configuration
{
    public class CandidatureConfiguration : IEntityTypeConfiguration<Candidature>
    {
        public void Configure(EntityTypeBuilder<Candidature> builder)
        {
            builder
                .HasKey(c => c.Id);

            builder.Property(c => c.Content)
                .HasMaxLength(CandidatureContentMaxLength)
                .IsRequired();

            builder
                .Property(c => c.Status)
                .HasDefaultValue(CandidatureStatus.Pending)
                .IsRequired();

            builder
                .HasOne(c => c.Project)
                .WithMany(p => p.Candidatures)
                .HasForeignKey(c => c.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(c => c.Applicant)
                .WithMany()
                .HasForeignKey(c => c.ApplicantId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Property(c => c.IsDeleted)
                .HasDefaultValue(false);
        }
    }
}
