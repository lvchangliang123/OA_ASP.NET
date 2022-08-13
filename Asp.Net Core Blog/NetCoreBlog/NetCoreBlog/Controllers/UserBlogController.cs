using BlogModels.ViewModels;
using Microsoft.AspNetCore.Mvc;
namespace NetCoreBlog.Controllers
{
    public class UserBlogController : Controller
    {

        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            var blogList = new List<NewBlogContentViewModel>()
            {
                new NewBlogContentViewModel{BlogTitle="Blog1",BlogContent="this is my blog1",BlogTags=new List<string>{"this is my blog1 tag" } },
                new NewBlogContentViewModel{BlogTitle="Blog2",BlogContent="this is my blog2",BlogTags=new List<string>{"this is my blog2 tag" } },
                new NewBlogContentViewModel{BlogTitle="Blog3",BlogContent="this is my blog3",BlogTags=new List<string>{"this is my blog3 tag" }}
            };
            return View(blogList);
        }

        [HttpGet]
        [Route("CreateNewBlog")]
        public IActionResult CreateNewBlog()
        {
            return View();
        }
    }
}
