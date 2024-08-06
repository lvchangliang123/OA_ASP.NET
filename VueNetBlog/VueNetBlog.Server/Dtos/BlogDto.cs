using VueNetBlog.Server.Models;

namespace VueNetBlog.Server.Dtos
{
    public class BlogDto
    {
        public string? Title { get; set; }

        public string? OverView { get; set; }

        public string? Content { get; set; }

        public List<string>? Tags { get; set; }

        public int UserId { get; set; } 
    }

}
