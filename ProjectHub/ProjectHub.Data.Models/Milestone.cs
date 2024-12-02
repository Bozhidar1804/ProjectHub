using System.ComponentModel.DataAnnotations;

using static ProjectHub.Common.EntityValidationConstants.Milestone;

namespace ProjectHub.Data.Models
{
	public class Milestone
	{
		[Key]
		public Guid Id { get; set; } = Guid.NewGuid();

		[Required]
		[MaxLength(MilestoneNameMaxLength)]
		public string Name { get; set; } = null!;

		public DateTime Deadline { get; set; }
		public bool IsCompleted {  get; set; }
		public bool IsDeleted { get; set; }

        // Computed property, dynamically calculating the milestone progress.
        // Not stored in the database, EF treats it like a read-only property
        // Avoids redundancy around the code
        public double Progress
        {
            get
            {
                if (Tasks == null || !Tasks.Any())
                {
                    return 0;
                }
                int completedTasks = Tasks.Count(t => t.IsCompleted);
                return (double)completedTasks / Tasks.Count * 100;
            }
        }

        [Required]
		public Guid ProjectId { get; set; }
		public Project Project { get; set; } = null!;

        public ICollection<Task> Tasks { get; set; } = new List<Task>();
    }
}
