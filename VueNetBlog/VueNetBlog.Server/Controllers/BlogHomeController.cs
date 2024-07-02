using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VueNetBlog.Server.Models;

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
            //[FromQuery] User user
            //var loginUser = user;
            //var redirectData = new Dictionary<string, object>
            //{
            //    { "UserName",loginUser.Name},
            //    {"UserAvatar", loginUser.Avatar},
            //};
            return Ok();
        }
    }

}
