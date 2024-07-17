namespace VueNetBlog.Server.Dtos
{
    public class BlogUploadImageDto
    {
        public string Title { get; set; }

        public int Position { get; set; }

        public IFormFile File { get; set; }
    }
}
