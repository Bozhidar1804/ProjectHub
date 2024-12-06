using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectHub.Services.Data.Interfaces
{
    public interface IVoteService
    {
        Task<bool> CanVoteAsync(Guid commentGuid, Guid userGuid);
        Task VoteUpvoteForCommentAsync(string commentId, string userId);
        Task VoteDownvoteForCommentAsync(string commentId, string userId);
    }
}
