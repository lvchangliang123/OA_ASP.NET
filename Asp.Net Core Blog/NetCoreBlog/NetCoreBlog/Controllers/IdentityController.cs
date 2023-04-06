using Microsoft.AspNetCore.Mvc;
using BlogModels.ViewModels;
using Microsoft.AspNetCore.Identity;
using NetCoreBlog.Models;
using BlogModels.ModelHelpers;

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
                var filePath = Path.Combine(avatarFlod, guidStr + "_" + registerViewModel.Avatar.FileName);
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


        [HttpGet]
        [Route("EditUserInfo")]
        public async Task<IActionResult> EditUserInfo()
        {
            var loginUser = await _signInManager.UserManager.GetUserAsync(HttpContext.User);
            if (loginUser == null)
            {
                await Response.WriteAsync("<script>alert('当前无登录用户，请先登录......')</script>");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var editUserVm = new EditUserInfoViewModel
                {
                    NewUserName = loginUser.UserName,
                    NewEmail = loginUser.Email,
                    NewBirthDay = loginUser.Birthday,
                    OldAvatar = "\\UserAvatars\\" + loginUser.Avatar,
                };
                return View(editUserVm);
            }
        }

        [HttpPost]
        [Route("EditUserInfo")]
        public async Task<IActionResult> EditUserInfo(EditUserInfoViewModel userInfo)
        {
            if (ModelState.IsValid)
            {
                //1.确认用户输入密码是否正确
                var loginUser = await _signInManager.UserManager.GetUserAsync(HttpContext.User);
                var result = await _signInManager.CheckPasswordSignInAsync(loginUser, userInfo.Password, false);
                if (result.Succeeded)
                {
                    loginUser.UserName = userInfo.NewUserName;
                    loginUser.Email = userInfo.NewEmail;
                    loginUser.Birthday = userInfo.NewBirthDay;
                    if (userInfo.NewAvatar != null)
                    {
                        //说明上传了新头像
                        var avatarFlod = Path.Combine(_webHostEnvironment.WebRootPath, "UserAvatars");
                        string guidStr = Guid.NewGuid().ToString();
                        var filePath = Path.Combine(avatarFlod, guidStr + "_" + userInfo.NewAvatar.FileName);

                        //删除原来的图片
                        var oldAvatarFile = new FileInfo(Path.Combine(avatarFlod, loginUser.Avatar));
                        if (oldAvatarFile.Exists)
                        {
                            oldAvatarFile.Delete();
                        }
                        //添加新图片
                        await userInfo.NewAvatar.CopyToAsync(new FileStream(filePath, FileMode.Create));
                        loginUser.Avatar = guidStr + "_" + userInfo.NewAvatar.FileName;
                    }
                    //2.密码正确，更新用户基本信息
                    var updateResult = await _userManager.UpdateAsync(loginUser);
                    if (updateResult.Succeeded)
                    {
                        //修改成功，返回首页
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        //错误返回当前页
                        return View("EditUserInfo");
                    }
                }
            }
            //错误返回当前页
            await Response.WriteAsync("<script>alert('修改用户信息失败，请检查信息是否正确......')</script>");
            return View("EditUserInfo");
        }


    }
}
