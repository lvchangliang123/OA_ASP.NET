using BlogModels.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogModels.Dtos
{
    public class BlogCommentDto : BlogCommentViewModel
    {
        [Key]
        public int Id { get; set; }
        public BlogInfoDto BlogInfoDto { get; set; }
        public int BlogInfoDtoId { get; set; }
    }
}
