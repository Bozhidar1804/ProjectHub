using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectHub.Common
{
    public static class EntityValidationConstants
    {
        public static class Project
        {
            public const int ProjectNameMaxLength = 30;
            public const int ProjectNameMinLength = 2;
            public const int ProjectDescriptionMaxLength = 500;
            public const int ProjectDescriptionMinLength = 15;
            public const string ProjectDatesFormat = "dd-MM-yyyy";
        }

		public static class Task
		{
			public const int TaskTitleMaxLength = 30;
			public const int TaskTitleMinLength = 2;
			public const int TaskDescriptionMaxLength = 300;
			public const int TaskDescriptionMinLength = 5;
			public const string TaskDueDateFormat = "dd-MM-yyyy";
		}
	}
}
