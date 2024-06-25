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

        public RegisController(UserManager<User> userManager, IRepository<User, int> userRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
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
            using var stream = userDto.Avatar.OpenReadStream();
            byte[] bytes = new byte[userDto.Avatar.Length];
            await stream.ReadAsync(bytes, 0, (int)userDto.Avatar.Length);
            var result = await RegisterUser(userDto, bytes);
            if (result)
            {
                return Ok("注册成功");
            }
            else
            {
                return StatusCode(500, "注册失败");
            }
        }

        private async Task<bool> RegisterUser(UserDto userDto, byte[] bytes)
        {
            try
            {
                var user_db = new User();
                user_db.Name = userDto.Name;
                user_db.UserName = userDto.Name;
                user_db.Email = userDto.Email;
                user_db.Password = userDto.Password;
                user_db.Birthday = DateTime.Parse(userDto.Birthday);
                user_db.Avatar = bytes;
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

    }

}
