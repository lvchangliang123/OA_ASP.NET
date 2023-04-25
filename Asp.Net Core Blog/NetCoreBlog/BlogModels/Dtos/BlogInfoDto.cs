using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [Description("文章封面")]
        public string BlogRelativeImageUrl { get; set; }
        [Description("点赞数量")]
        public int GiveLikeCount { get; set; }
        [Description("访问数量")]
        public int ViewCount { get; set; }
        [Description("博客概要")]
        public string BlogSummary { get; set; } = "";
        //一对多
        public IList<BlogCommentDto> BlogComments { get; set; }=new List<BlogCommentDto>();
        public ICollection<BlogCollection> BlogCollections { get; set; }=new List<BlogCollection>();
    }
}
