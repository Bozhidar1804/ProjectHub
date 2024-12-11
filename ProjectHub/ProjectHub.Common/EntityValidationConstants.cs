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
			public const int TaskTitleMaxLength = 100;
			public const int TaskTitleMinLength = 2;
			public const int TaskDescriptionMaxLength = 300;
			public const int TaskDescriptionMinLength = 5;
			public const string TaskDueDateFormat = "dd-MM-yyyy";
		}

        public static class ApplicationUser
        {
            public const int ApplicationUserFullNameMaxLength = 50;
			public const int ApplicationUserFullNameMinLength = 5;
            public const int ApplicationUserPasswordMaxLength = 100;
            public const int ApplicationUserPasswordMinLength = 6;
            // Email constants still not used
            public const int ApplicationUserEmailMaxLength = 254;
			public const int ApplicationUserEmailMinLength = 6;
		}

		public static class Checklist
		{
			public const int ChecklistContentMaxLength = 100;
			public const int ChecklistContentMinLength = 5;
		}

		public static class Milestone
		{
			public const int MilestoneNameMaxLength = 30;
			public const int MilestoneNameMinLength = 3;
		}

		public static class Comment
		{
			public const int CommentContentMaxLength = 300;
			public const int CommentContentMinLength = 5;
			public const int CommentVotesDefaultValue = 0;
		}

		public static class ActivityLog
		{
			public const int ActivityLogReasonMaxLength = 100;
			public const int ActivityLogReasonMinLength = 5;
		}

		public static class Tag
		{
			public const int TagNameMaxLength = 20;
			public const int TagNameMinLength = 2;
		}

		public static class Candidature
		{
			// public const int CandidatureContentMaxLength = 2000; NOT USED CURRENTLY
		}
	}
}
