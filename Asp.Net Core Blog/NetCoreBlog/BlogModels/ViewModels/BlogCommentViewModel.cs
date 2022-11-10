using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogModels.ViewModels
{
    public class BlogCommentViewModel
    {
        public string CommentUserName { get; set; }
        public string CommentUserEmail { get; set; }
        public string CommentUserAvatar { get; set; }
        public string CommentBody { get; set; }
        public DateTime CommentDate { get; set; }

    }
}
