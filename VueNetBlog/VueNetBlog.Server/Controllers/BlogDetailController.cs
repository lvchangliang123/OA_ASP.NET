using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VueNetBlog.Server.DataBaseFramework;
using VueNetBlog.Server.Dtos;
using VueNetBlog.Server.Models;

namespace VueNetBlog.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogDetailController : ControllerBase
    {
        private IRepository<Blog, int> _blogRepository;
        private IRepository<User, int> _userRepository;
        private IRepository<Comment, int> _commentRepository;

        public BlogDetailController(IRepository<Blog, int> blogRepository, IRepository<User, int> userRepository, IRepository<Comment, int> commentRepository)
        {
            _blogRepository = blogRepository;
            _userRepository = userRepository;
            _commentRepository = commentRepository;
        }

        [HttpGet]
        [Route("GetBlogData")]
        public async Task<IActionResult> GetBlogData(int userid, int blogid)
        {
            var blog = await _blogRepository.SingleAsync(b => b.Id == blogid && b.UserId == userid);
            if (blog.ViewCount==null)
            {
                blog.ViewCount = 1;
            }
            else
            {
                blog.ViewCount++;
            }
            await _blogRepository.UpdateAsync(blog);
            blog.Comments = await _commentRepository.GetAllListAsync(c => c.BlogId == blogid);
            foreach (var com in blog.Comments)
            {
                com.Blog = blog;
                com.User = _userRepository.FirstOrDefault(u => u.Id == com.UserId);
            }
            if (blog != null)
            {
                if (blog.User == null && userid != 0)
                {
                    blog.User = _userRepository.FirstOrDefault(u => u.Id == userid);
                }
                return Ok(blog);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("SubmitBlogComment")]
        public async Task<IActionResult> SubmitBlogComment([FromForm] BlogCommentDto blogCommentDto)
        {
            var blog = await _blogRepository.SingleAsync(b => b.Id == blogCommentDto.BlogId);
            var commentUser = _userRepository.FirstOrDefault(u => u.Id == blogCommentDto.UserId);
            if (blog != null)
            {
                blog.Comments.Add(new Comment() { Content = blogCommentDto.CommentContent, User = commentUser, Blog = blog });
                await _blogRepository.UpdateAsync(blog);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

    }
}
