using BlogModels.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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


        //为分页准备一些变量
        [BindProperty(SupportsGet = true)]
        [NotMapped]
        public int CurrentPage { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        [NotMapped]
        public int OnePageCount { get; set; } = 1;

        [NotMapped]
        public int TotalCommentCounts
        {
            get
            {
                return BlogComments == null ? 0 : BlogComments.Count;
            }
        }
    }
}
