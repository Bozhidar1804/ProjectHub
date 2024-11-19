using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
