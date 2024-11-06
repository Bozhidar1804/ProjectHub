using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectHub.Data.Models;

namespace ProjectHub.Data.Configuration
{
    public class ChecklistConfiguration : IEntityTypeConfiguration<CheckList>
    {
        public void Configure(EntityTypeBuilder<CheckList> builder)
        {
            builder
                .HasKey(c => c.Id);

            builder
                .Property(c => c.TaskId)
                .IsRequired();

            builder
                .HasOne(c => c.Task)
                .WithMany(t => t.Checklists)
                .HasForeignKey(c => c.TaskId);

            builder
                .Property(m => m.IsDeleted)
                .HasDefaultValue(false);
        }
    }
}
