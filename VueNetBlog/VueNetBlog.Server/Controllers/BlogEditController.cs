using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace VueNetBlog.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogEditController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        public BlogEditController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost]
        [Route("UploadBlogImage")]
        public async Task<IActionResult> UploadBlogImage(IFormFile file)
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
        [Route("UploadBlogContent")]
        public async Task<IActionResult> Post([FromBody] ArticleModel model)
        {
            _dbContext.Articles.Add(model);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetArticleById), new { id = model.Id }, model);
        }

    }
}
