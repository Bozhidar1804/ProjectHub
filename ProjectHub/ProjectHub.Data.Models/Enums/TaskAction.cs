using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectHub.Data.Models.Enums
{
	public enum TaskAction
	{
		Created = 0,
		Updated = 1,
		Completed = 2,
		Assigned = 3,
		PriorityChanged = 4,
		CommentAdded = 5
	}
}
