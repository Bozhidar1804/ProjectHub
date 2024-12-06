using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectHub.Data.Models;

namespace ProjectHub.Data.Configuration
{
    public class VoteConfiguration : IEntityTypeConfiguration<Vote>
    {
        public void Configure(EntityTypeBuilder<Vote> builder)
        {
            
            builder
                .HasKey(v => v.Id);

            builder
                .Property(v => v.IsUpvote)
                .IsRequired();

            builder
                .HasOne(v => v.Comment)
                .WithMany(c => c.Votes)
                .HasForeignKey(v => v.CommentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(v => v.User)
                .WithMany(u => u.Votes)
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
