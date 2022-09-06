using BlogModels.ViewModels;
using BlogModels.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using DataBaseFramework.Infrastructure;

namespace NetCoreBlog.Controllers
{
    public class UserBlogController : Controller
    {
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;
        private IRepository<BlogInfoDto, int> _blogRepository;
        public UserBlogController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IRepository<BlogInfoDto, int> blogRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _blogRepository = blogRepository;
        }

        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            //查询数据库，得到当前用户的所有博客信息
            var blogList = _blogRepository.GetAll().Where(b=>b.UserId== _signInManager.UserManager.GetUserAsync(HttpContext.User).Result.Id);
            return View(blogList?.ToList());
        }

        [HttpGet]
        [Route("CreateNewBlog")]
        public IActionResult CreateNewBlog()
        {
            return View();
        }

        [HttpPost]
        [Route("CreateNewBlog")]
        public IActionResult CreateNewBlog(NewBlogContentViewModel model)
        {
            if (ModelState.IsValid)
            {
                //1.创建模型
                var blogInfoDto = new BlogInfoDto()
                {
                    BlogTitle = model.BlogTitle,
                    BlogContent = model.BlogContent,
                    UserId= _signInManager.UserManager.GetUserAsync(HttpContext.User).Result.Id,
                    UserName = _signInManager.UserManager.GetUserAsync(HttpContext.User).Result.UserName,
                    ModifyTime = DateTime.Now,
                    BlogTags = model.BlogTags,
                };
                //2.利用仓储模式，存储数据库
                _blogRepository.Insert(blogInfoDto);
                //3.存储完成后，回到当前登录用户的博客列表页面
                return RedirectToAction("Index");
            }
            return View();
        }

    }
}
