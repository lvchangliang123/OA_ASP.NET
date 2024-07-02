using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text.RegularExpressions;
using VueNetBlog.Server.Dtos;
using VueNetBlog.Server.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace VueNetBlog.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private SignInManager<User> _signInManager;

        private UserManager<User> _userManager;
        public LoginController(UserManager<User> userManager,SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        [Route("UserLogin")]
        public async Task<IActionResult> UserLogin([FromForm] UserLoginDto userLoginDto)
        {
            if (ModelState.IsValid)
            {
                string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                Regex regex = new Regex(pattern);
                if (regex.IsMatch(userLoginDto.Identifier))
                {
                    var user = await _userManager.FindByEmailAsync(userLoginDto.Identifier);
                    if (user != null)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, userLoginDto.Password, false, lockoutOnFailure: false);
                        if (result.Succeeded)
                        {
                            var redirectData = new Dictionary<string, object>
                            {
                                { "PageName","RedirectToBlogHome"},
                                {"User", user},
                            };
                            return Ok(redirectData);
                        }
                        else if (result.IsLockedOut)
                        {
                            return StatusCode(500, "User Lock out");
                        }
                        else
                        {
                            return StatusCode(500, "服务器请求失败");
                        }
                    }
                    else
                    {
                        return StatusCode(500, "用户不存在");
                    }
                }
                else
                {
                    var result = await _signInManager.PasswordSignInAsync(userLoginDto.Identifier, userLoginDto.Password, false, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        return Ok("RedirectToBlogHome");
                    }
                    else if (result.IsLockedOut)
                    {
                        return StatusCode(500, "User Lock out");
                    }
                    else
                    {
                        return StatusCode(500, "服务器请求失败");
                    }
                }
            }
            else
            {
                return StatusCode(500, "服务器请求失败");
            }
        }

    }
}
