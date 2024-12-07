using System.ComponentModel.DataAnnotations;

namespace ProjectHub.Data.Models.Enums
{
	public enum ProjectStatus
	{
        [Display(Name = "In Progress")]
        InProgress = 0,

        [Display(Name = "Completed")]
        Completed = 1
    }
}
