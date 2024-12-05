using ProjectHub.Data.Models;
using ProjectHub.Web.ViewModels.Comment;

namespace ProjectHub.Services.Data.Interfaces
{
    public interface ICommentService
    {
        Task<bool> AddCommentAsync(AddCommentFormModel model, string userId);
        Task<List<Comment>> GetCommentsByTaskIdAsync(string taskId);

        System.Threading.Tasks.Task UpvoteCommentAsync(string commentId);
        System.Threading.Tasks.Task DownvoteCommentAsync(string commentId);
    }
}
