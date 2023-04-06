using BlogModels.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogModels.ViewModels
{
    public class BlogContentViewModel : NewBlogContentViewModel
    {
        public List<string> BlogTagList { get; set; }
        public BlogCommentDto NewBlogComment { get; set; }
        public List<BlogCommentDto> BlogComments { get; set; }
        public int GiveLikeCount { get; set; }
        public int ViewCount { get; set; }
        public int CollectCount { get; set; }
    }
}
