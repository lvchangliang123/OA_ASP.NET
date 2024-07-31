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

        public AboutController(IRepository<Blog, int> blogRepository)
        {
            _blogRepository = blogRepository;
        }

        [HttpGet]
        [Route("GetUserBlogData")]
        public async Task<IActionResult> GetUserBlogData(int userid)
        {
            var blogs = await _blogRepository.GetAllListAsync(b => b.Id == userid);
            if (blogs != null)
            {
                return Ok(blogs.OrderByDescending(b => b.CreateTime).Take(5).ToList());
            }
            else
            {
                return Ok(null);
            }
        }
    }
}
