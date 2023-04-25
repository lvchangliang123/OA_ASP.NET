using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogModels.Dtos
{
    public class HomeBlogInfosDto
    {
        public List<BlogInfoDto> BlogInfoDtos { get; set; }
        public int TotalUsers { get; set; }
        public int TotalBlogs { get; set; }
        public int TotalKinds { get; set; }
        public int TotalTags { get; set; }
        public int TotalComments { get; set; }
        public int TotalBlogViews { get; set; }
        public List<BlogInfoDto> MostPopularBlogs { get; set; }
        public List<BlogCommentDto> MostNewBlogComments { get; set; }
    }
}
