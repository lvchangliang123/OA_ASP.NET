using BlogModels.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogModels.ViewModels
{
    public class NewBlogContentViewModel
    {
        public int? BlogId { get; set; }
        [Required]
        public string BlogTitle { get; set; }
        [Required]
        public string BlogContent { get; set; }
        [Required]
        public string BlogTags { get; set; }
        public List<string> BlogTagList { get; set; }
        public BlogCommentDto NewBlogComment { get; set; }
        public List<BlogCommentDto> BlogComments { get; set; }

    }
}
