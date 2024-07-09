using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VueNetBlog.Server.DataBaseFramework;
using VueNetBlog.Server.Dtos;
using VueNetBlog.Server.Models;

namespace VueNetBlog.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisController : ControllerBase
    {
        private IRepository<User, int> _userRepository;

        private UserManager<User> _userManager;

        private readonly IWebHostEnvironment _env;

        public RegisController(UserManager<User> userManager, IRepository<User, int> userRepository, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _env = env; 
        }

        [HttpPost]
        [Route("RegisUser")]
        public async Task<IActionResult> RegisUser([FromForm] UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (userDto.Avatar == null || userDto.Avatar.Length == 0)
            {
                return BadRequest("头像未上传");
            }
            var result = await RegisterUser(userDto, userDto.Avatar);
            if (result)
            {
                return Ok("注册成功");
            }
            else
            {
                return StatusCode(500, "注册失败");
            }
        }

        private async Task<bool> RegisterUser(UserDto userDto, IFormFile avatar)
        {
            try
            {
                var user_db = new User();
                user_db.Name = userDto.Name;
                user_db.UserName = userDto.Name;
                user_db.Email = userDto.Email;
                user_db.Password = userDto.Password;
                user_db.Birthday = DateTime.Parse(userDto.Birthday);
                user_db.AvatarPath = SaveAvatar(avatar);
                var identityResult = await _userManager.CreateAsync(user_db, user_db.Password);
                if (identityResult.Succeeded)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string SaveAvatar(IFormFile file)
        {
            string uploadsDir = Path.Combine(_env.ContentRootPath, "uploads");
            string avatarsDir = Path.Combine(uploadsDir, "avatars");
            if (!Directory.Exists(avatarsDir))
            {
                Directory.CreateDirectory(avatarsDir);
            }
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(avatarsDir, fileName);
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fs);
            }
            return Path.Combine("uploads", "avatars", fileName);
        }

    }

}
