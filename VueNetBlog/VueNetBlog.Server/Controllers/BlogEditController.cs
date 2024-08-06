using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VueNetBlog.Server.DataBaseFramework;
using VueNetBlog.Server.Dtos;
using VueNetBlog.Server.Models;

namespace VueNetBlog.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogEditController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        private IRepository<Blog, int> _blogRepository;
        private IRepository<User, int> _userRepository;

        private SignInManager<User> _signInManager;

        public BlogEditController(IWebHostEnvironment env, IRepository<Blog, int> blogRepository, SignInManager<User> signInManager, IRepository<User, int> userRepository)
        {
            _env = env;
            _blogRepository = blogRepository;
            _signInManager = signInManager;
            _userRepository = userRepository;
        }

        [HttpPost]
        [Route("UploadBlogImage")]
        public async Task<IActionResult> UploadBlogImage([FromForm] BlogUploadImageDto blogUploadImageDto)
        {
            if (blogUploadImageDto.File.Length > 0)
            {
                string uploadsDir = Path.Combine(_env.ContentRootPath, "uploads");
                string avatarsDir = Path.Combine(uploadsDir, "blogImages");
                if (!Directory.Exists(avatarsDir))
                {
                    Directory.CreateDirectory(avatarsDir);
                }
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(blogUploadImageDto.File.FileName);
                string filePath = Path.Combine(avatarsDir, fileName);
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    await blogUploadImageDto.File.CopyToAsync(fs);
                }
                string url = Path.Combine("uploads", "blogImages", fileName);
                url = $"{Request.Scheme}://{Request.Host}/{url}".Replace("\\", "/");
                return Ok(new { url });
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("DeleteBlogImage")]
        public IActionResult DeleteBlogImage([FromForm] BlogDeleteImageDto blogDeleteImageDto)
        {
            if (blogDeleteImageDto.FileName.Length > 0)
            {
                string filePath = Path.Combine(_env.ContentRootPath,"uploads",blogDeleteImageDto.FileName.Split("uploads/")[1]);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
               return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("UploadBlogContent")]
        public async Task<IActionResult> UploadBlogContent([FromForm] BlogDto blogDto)
        {
            var blog_db = new Blog();
            blog_db.Title = blogDto.Title;
            blog_db.OverView = blogDto.OverView;
            blog_db.Content = blogDto.Content;
            blog_db.Tags = string.Join(';', blogDto.Tags);
            blog_db.User = _userRepository.FirstOrDefault(u => u.Id == blogDto.UserId);
            await _blogRepository.InsertAsync(blog_db);
            return Ok();
        }

    }
}
