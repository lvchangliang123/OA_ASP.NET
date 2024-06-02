using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace VueNetBlog.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogHomeController : ControllerBase
    {
        [HttpGet]
        [Route("BlogHomeView")]
        public IActionResult BlogHomeView()
        {
            return Ok("123");
        }
    }
}
