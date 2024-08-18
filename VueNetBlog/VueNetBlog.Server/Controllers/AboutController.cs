using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VueNetBlog.Server.DataBaseFramework;
using VueNetBlog.Server.Models;

namespace VueNetBlog.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AboutController : ControllerBase
    {
        private IRepository<Blog, int> _blogRepository;
        private IRepository<User, int> _userRepository;

        public AboutController(IRepository<Blog, int> blogRepository, IRepository<User, int> userRepository)
        {
            _blogRepository = blogRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        [Route("GetUserBlogData")]
        public async Task<IActionResult> GetUserBlogData(int userid)
        {
            var blogs = await _blogRepository.GetAllListAsync(b => b.UserId == userid);
            if (blogs.Any())
            {
                if (blogs.Count > 5)
                {
                    return Ok(blogs.OrderByDescending(b => b.CreateTime).Take(5).ToList());
                }
                else
                {
                    return Ok(blogs.OrderByDescending(b => b.CreateTime).ToList());
                }
            }
            else
            {
                return Ok(null);
            }
        }
    }
}
