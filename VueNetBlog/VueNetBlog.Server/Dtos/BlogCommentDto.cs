namespace VueNetBlog.Server.Dtos
{
    public class BlogCommentDto
    {
        public int BlogId { get; set; }

        public int UserId { get; set; }

        public string? CommentContent { get; set; }
    }
}
