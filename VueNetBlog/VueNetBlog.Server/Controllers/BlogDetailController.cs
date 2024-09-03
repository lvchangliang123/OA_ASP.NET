using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VueNetBlog.Server.DataBaseFramework;
using VueNetBlog.Server.Models;

namespace VueNetBlog.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogDetailController : ControllerBase
    {
        private IRepository<Blog, int> _blogRepository;

        public BlogDetailController(IRepository<Blog, int> blogRepository)
        {
            _blogRepository = blogRepository;
        }

        [HttpGet]
        [Route("GetBlogData")]
        public async Task<IActionResult> GetBlogData(int userid, int blogid)
        {
            var blog = await _blogRepository.SingleAsync(b => b.Id == blogid && b.UserId == userid);
            if (blog!=null)
            {
                return Ok(blog);
            }
            else
            {
                return BadRequest();
            }
        }

    }
}
