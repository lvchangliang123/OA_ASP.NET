using BlogModels.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        [Description("文章封面")]
        public IFormFile BlogRelativeImage { get; set; }

    }
}
