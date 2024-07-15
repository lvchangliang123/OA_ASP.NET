namespace VueNetBlog.Server.Dtos
{
    public class BlogDto
    {
        public string? Title { get; set; }

        public string? OverView { get; set; }

        public string? Content { get; set; }

        public string[]? Tags { get; set; }
    }
}
