using Microsoft.AspNetCore.Mvc;
using BlogModels.ViewModels;
using Microsoft.AspNetCore.Identity;
using NetCoreBlog.Models;
using DataBaseFramework.Models;

namespace NetCoreBlog.Controllers
{
    public class IdentityController : Controller
    {
        private UserManager<CustomerIdentityUser> _userManager;
        private SignInManager<CustomerIdentityUser> _signInManager;
        private IWebHostEnvironment _webHostEnvironment;

        public IdentityController(UserManager<CustomerIdentityUser> userManager, SignInManager<CustomerIdentityUser> signInManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        [Route("RegisterUser")]
        public IActionResult RegisterUser()
        {
            return View();
        }

        [HttpPost]
        [Route("RegisterUser")]
        public async Task<IActionResult> RegisterUser(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                //将头像上传到root文件夹下的UserAvatars文件夹下
                //创建identityUser对象
                var avatarFlod = Path.Combine(_webHostEnvironment.WebRootPath, "UserAvatars");
                string guidStr = Guid.NewGuid().ToString();
                var filePath = Path.Combine(avatarFlod,guidStr + "_" + registerViewModel.Avatar.FileName);
                var user = new CustomerIdentityUser
                {
                    UserName = registerViewModel.UserName,
                    Email = registerViewModel.Email,
                    Birthday = registerViewModel.BirthDay,
                    Avatar = guidStr + "_" + registerViewModel.Avatar.FileName,
                };
                //上传头像
                await registerViewModel.Avatar.CopyToAsync(new FileStream(filePath, FileMode.Create));
                var result = await _userManager.CreateAsync(user, registerViewModel.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");   //回到主页
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(registerViewModel);
        }


        [HttpGet]
        [Route("ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(EmailAddressViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                //var confirmResult = await _userManager.IsEmailConfirmedAsync(user);
                //这个地方需要发送邮件，确认验证码后才能修改密码！！！
                if (user != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    return RedirectToAction("ModifyPassword", "Identity", new ModifyPasswordViewModel { Email = model.Email, Token = token });
                }
                return View(model);
            }
            return View(model);
        }


        [HttpGet]
        [Route("ModifyPassword")]
        public IActionResult ModifyPassword(string token, string email)
        {
            if (token == null || email == null)
            {
                ModelState.AddModelError("", "无效的密码重置令牌");
            }
            return View();
        }


        [HttpPost]
        [Route("ModifyPassword")]
        public async Task<IActionResult> ModifyPassword(ModifyPasswordViewModel modifyPasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(modifyPasswordViewModel.Email);
                if (user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, modifyPasswordViewModel.Token, modifyPasswordViewModel.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ResetpasswordSuccess", "Identity");
                    }
                }
            }
            return View(modifyPasswordViewModel);
        }

        [HttpGet]
        [Route("ResetpasswordSuccess")]
        public IActionResult ResetpasswordSuccess()
        {
            return View();
        }

        [HttpGet]
        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password, loginViewModel.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "登陆失败，请重试");
            }
            return View(loginViewModel);
        }

        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }



    }
}
