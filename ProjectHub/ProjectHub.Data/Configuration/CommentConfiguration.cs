using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ProjectHub.Data.Models;
using static ProjectHub.Common.EntityValidationConstants.Comment;

namespace ProjectHub.Data.Configuration
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder
                .HasKey(c => c.Id);

            builder
                .Property(c => c.Upvotes)
                .IsRequired()
                .HasDefaultValue(CommentVotesDefaultValue);

            builder
                .Property(c => c.Downvotes)
                .IsRequired()
                .HasDefaultValue(CommentVotesDefaultValue);

            builder
                .Property(c => c.TaskId)
                .IsRequired();

            builder
                .HasOne(c => c.Task)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(c => c.PostedByUser)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.PostedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Property(m => m.IsDeleted)
                .HasDefaultValue(false);
        }
    }
}
