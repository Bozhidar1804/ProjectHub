using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectHub.Data.Models
{
    public class Vote
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public bool IsUpvote { get; set; } // True for upvote, false for downvote
        public DateTime CreatedOn { get; set; }

        [Required]
        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        [Required]
        public Guid CommentId { get; set; }
        [ForeignKey(nameof(CommentId))]
        public Comment Comment { get; set; } = null!;
    }
}
