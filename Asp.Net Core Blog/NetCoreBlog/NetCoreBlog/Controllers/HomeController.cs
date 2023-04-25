using BlogModels.Dtos;
using BlogModels.ModelHelpers;
using DataBaseFramework.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCoreBlog.Models;
using System.Diagnostics;
using System.Linq;

namespace NetCoreBlog.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private UserManager<CustomerIdentityUser> _userManager;
        private IRepository<BlogInfoDto, int> _blogRepository;
        private IRepository<BlogCommentDto, int> _blogCommentRepository;

        public HomeController(ILogger<HomeController> logger, UserManager<CustomerIdentityUser> userManager, IRepository<BlogInfoDto, int> blogRepository, IRepository<BlogCommentDto, int> blogCommentRepository)
        {
            //_logger = logger;
            _userManager = userManager;
            _blogRepository = blogRepository;
            _blogCommentRepository = blogCommentRepository;
        }

        public async Task<IActionResult> Index()
        {
            var homeInfosDto = new HomeBlogInfosDto();
            //博客数量太多要考虑分批查询并分页
            var userIds = _userManager.Users.Select(u => u.Id).ToList();
            var totalBlogs = await _blogRepository.GetAllListAsync();
            var displayBlogs = totalBlogs.OrderBy(b=>b.ModifyTime).Take(3).ToList();
            homeInfosDto.BlogInfoDtos = displayBlogs; 
            homeInfosDto.TotalUsers= _userManager.Users.Count();
            homeInfosDto.TotalBlogs= _blogRepository.Count();
            homeInfosDto.TotalKinds = 3;
            homeInfosDto.TotalTags = Enum.GetNames((new BlogModels.Enum.EnumBlogTag()).GetType()).Length;
            homeInfosDto.TotalComments = _blogCommentRepository.Count();
            homeInfosDto.TotalBlogViews = totalBlogs.Select(b => b.ViewCount).Sum();
            homeInfosDto.MostPopularBlogs= totalBlogs.OrderBy(b=>b.GiveLikeCount).Take(3).ToList();
            var totalComments = await _blogCommentRepository.GetAllListAsync();
            homeInfosDto.MostNewBlogComments = totalComments.OrderBy(c=>c.CommentDate).Take(3).ToList();
            return View(homeInfosDto);
        }

        [HttpPost]
        public async Task<JsonResult> GetBlogInfosByPage([FromBody] GetBlogInfosHelper blogInfosHelper)
        {
            var totalBlogs = await _blogRepository.GetAllListAsync();
            var displayBlogs = totalBlogs.OrderBy(b => b.ModifyTime)
                .Skip((blogInfosHelper.pageNumber - 1) * blogInfosHelper.pageSize)
                .Take(blogInfosHelper.pageSize)
                .ToList();
            return Json(displayBlogs);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}