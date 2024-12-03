using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectHub.Common
{
    public static class NotificationMessagesConstants
    {
        public const string GeneralErrorMessage = "{0} must be between {1} and {2} characters.";

        public static class User
        {
            public const string ErrorMessage = "ErrorMessage";
        }

        public static class Candidature
        {
            public const string AnswerRequiredMessage = "Answer cannot be empty.";
        }
    }
}
