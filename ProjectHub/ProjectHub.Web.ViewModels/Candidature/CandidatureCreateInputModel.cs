using System.ComponentModel.DataAnnotations;

using static ProjectHub.Common.NotificationMessagesConstants.Candidature;

namespace ProjectHub.Web.ViewModels.Candidature
{
    public class CandidatureCreateInputModel
    {
        public Guid ProjectId { get; set; }

        [Required(ErrorMessage = AnswerRequiredMessage)]
        public string Answer1 { get; set; } = null!; // What interests you about this project?

        [Required(ErrorMessage = AnswerRequiredMessage)]
        public string Answer2 { get; set; } = null!; // What skills can you contribute?

        [Required(ErrorMessage = AnswerRequiredMessage)]
        public string Answer3 { get; set; } = null!; // Why should we choose you?

        [Required(ErrorMessage = AnswerRequiredMessage)]
        public string Answer4 { get; set; } = null!; // What are your goals for joining this project?
    }
}
