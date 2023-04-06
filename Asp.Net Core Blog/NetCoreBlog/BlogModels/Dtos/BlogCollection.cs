using BlogModels.ModelHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogModels.Dtos
{
    public class BlogCollection
    {
        [Key]
        public int Id { get; set; }
        public BlogInfoDto BlogInfoDto { get; set; } 
        public CustomerIdentityUser CustomerIdentityUser { get; set; }
    }
}
