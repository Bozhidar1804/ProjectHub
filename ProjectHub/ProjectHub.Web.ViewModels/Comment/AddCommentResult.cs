using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectHub.Web.ViewModels.Comment
{
    public class AddCommentResult
    {
        public bool Success { get; set; }
        public string? TaskId { get; set; }
        public string? ErrorMessage {  get; set; }
    }
}
