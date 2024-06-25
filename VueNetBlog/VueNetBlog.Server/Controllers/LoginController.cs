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
                            return RedirectToAction("Index", "Home");
                        }
                        else if (result.IsLockedOut)
                        {
                            return RedirectToAction("Lockout");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                            return View(model);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "User with this email does not exist.");
                        return View(model);
                    }
                }
                else
                {
                    var result = await _signInManager.PasswordSignInAsync(userLoginDto.Identifier, userLoginDto.Password, false, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        _logger.LogWarning($"用户{loginViewModel.UserName}登录成功");
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        _logger.LogError($"用户{loginViewModel.UserName}登录失败");
                        ModelState.AddModelError(string.Empty, "登陆失败，请重试");
                    }
                }
            }
        }

    }
}
