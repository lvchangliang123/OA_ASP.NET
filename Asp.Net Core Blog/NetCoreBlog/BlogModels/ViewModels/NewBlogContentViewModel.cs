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
        [Required]
        public string BlogTitle { get; set; }
        [Required]
        public string BlogContent { get; set; }
        [Required]
        public string BlogTags { get; set; }
    }
}
