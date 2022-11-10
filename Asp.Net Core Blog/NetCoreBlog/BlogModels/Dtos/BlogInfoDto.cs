using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogModels.Dtos
{
    public class BlogInfoDto 
    {
        [Key]
        public int Id { get; set; }

        //应该有UserId
        public string UserId { get; set; }

        public string UserName { get; set; }
        //用户头像路径
        public string Avatar { get; set; }
        public DateTime ModifyTime { get; set; }
        public string BlogTitle { get; set; }
        public string BlogContent { get; set; }
        public string BlogTags { get; set; }
        //一对多
        public List<BlogCommentDto> BlogComments { get; set; }=new List<BlogCommentDto>();
    }
}
