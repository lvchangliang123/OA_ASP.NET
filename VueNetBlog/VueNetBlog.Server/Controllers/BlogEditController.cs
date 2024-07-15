using Microsoft.AspNetCore.Http;
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

        public BlogEditController(IWebHostEnvironment env, IRepository<Blog, int> blogRepository)
        {
            _env = env;
            _blogRepository = blogRepository;
        }

        [HttpPost]
        [Route("UploadBlogImage")]
        public async Task<IActionResult> UploadBlogImage(string title, int pos, IFormFile file)
        {
            if (file.Length > 0)
            {
                string uploadsDir = Path.Combine(_env.ContentRootPath, "uploads");
                string avatarsDir = Path.Combine(uploadsDir, "blogImages");
                if (!Directory.Exists(avatarsDir))
                {
                    Directory.CreateDirectory(avatarsDir);
                }
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(avatarsDir, fileName);
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fs);
                }
                string url = Path.Combine("uploads", "blogImages", fileName);
                return Ok(new { url });
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("DeleteBlogImage")]
        public IActionResult DeleteBlogImage(string title, int pos, IFormFile file)
        {
            if (file.Length > 0)
            {
                string uploadsDir = Path.Combine(_env.ContentRootPath, "uploads");
                string avatarsDir = Path.Combine(uploadsDir, "blogImages");
                if (!Directory.Exists(avatarsDir))
                {
                    Directory.CreateDirectory(avatarsDir);
                }
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(avatarsDir, fileName);
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
        public async Task<IActionResult> UploadBlogContent([FromBody] BlogDto blogDto)
        {
            var blog_db = new Blog();
            blog_db.Title = blogDto.Title;
            blog_db.OverView = blogDto.OverView;
            blog_db.Content = blogDto.Content;
            blog_db.Tags = string.Join(';', blogDto.Tags);
            await _blogRepository.InsertAsync(blog_db);
            return Ok();
        }

    }
}
