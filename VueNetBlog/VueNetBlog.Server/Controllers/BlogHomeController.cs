using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VueNetBlog.Server.DataBaseFramework;
using VueNetBlog.Server.Models;

namespace VueNetBlog.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogHomeController : ControllerBase
    {
        private IRepository<Blog, int> _blogRepository;
        private IRepository<User, int> _userRepository;
        private IRepository<Comment, int> _commentRepository;

        public BlogHomeController(IRepository<Blog, int> blogRepository, IRepository<Comment, int> commentRepository, IRepository<User, int> userRepository)
        {
            _blogRepository = blogRepository;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        [Route("BlogHomeView")]
        public IActionResult BlogHomeView()
        {
            //[FromQuery] User user
            //var loginUser = user;
            //var redirectData = new Dictionary<string, object>
            //{
            //    { "UserName",loginUser.Name},
            //    {"UserAvatar", loginUser.Avatar},
            //};
            return Ok();
        }

        [HttpGet]
        [Route("GetHomeBlogs")]
        public async Task<IActionResult> GetHomeBlogs()
        {
            var blogs = await _blogRepository.GetAllListAsync();
            var selectBlogs = blogs.OrderByDescending(b => b.ViewCount).Take(3);
            foreach (var blog in selectBlogs)
            {
                if (blog.User == null && blog.UserId != 0)
                {
                    blog.User = _userRepository.FirstOrDefault(u => u.Id == blog.UserId);
                }
                blog.Comments = await _commentRepository.GetAllListAsync(c => c.BlogId == blog.Id);
                foreach (var com in blog.Comments)
                {
                    com.Blog = blog;
                    com.User = _userRepository.FirstOrDefault(u => u.Id == com.UserId);
                }
            }
            if (selectBlogs.Any())
            {
                return Ok(selectBlogs);
            }
            else
            {
                return BadRequest();
            }
        }

    }

}
