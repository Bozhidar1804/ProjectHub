using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

using static ProjectHub.Common.EntityValidationConstants.Candidature;
using ProjectHub.Data.Models.Enums;

namespace ProjectHub.Data.Models
{
    public class Candidature
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string Content { get; set; } = null!;

        [Required]
        public Guid ProjectId { get; set; }

        [ForeignKey(nameof(ProjectId))]
        public Project Project { get; set; } = null!;

        [Required]
        public Guid ApplicantId { get; set; }

        [ForeignKey(nameof(ApplicantId))]
        public ApplicationUser Applicant { get; set; } = null!;

        [Required]
        public CandidatureStatus Status { get; set; } = CandidatureStatus.Pending;

        [Required]
        public DateTime ApplicationDate { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; }
    }
}
