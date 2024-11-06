﻿using System.ComponentModel.DataAnnotations;

using ProjectHub.Data.Models.Enums;
using static ProjectHub.Common.EntityValidationConstants.Project;

namespace ProjectHub.Data.Models
{
	public class Project
	{
		[Key]
		public Guid Id { get; set; } = Guid.NewGuid();

		[Required]
		[MaxLength(ProjectNameMaxLength)]
		public string Name { get; set; } = null!;

		[Required]
		[MaxLength(ProjectDescriptionMaxLength)]
		public string Description { get; set; } = null!;
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public ProjectStatus Status { get; set; }
		public bool IsDeleted { get; set; }

		public ICollection<Task> Tasks { get; set; } = new List<Task>();
		public ICollection<Milestone> Milestones { get; set; } = new List<Milestone>();
		public ICollection<User> TeamMembers { get; set; } = new List<User>();
	}
}