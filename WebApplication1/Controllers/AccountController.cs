using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MrHuo.OAuth.Github;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    [Route("Account")]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private SignInManager<ApplicationUser> signInManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger<AdminController> logger;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IHttpContextAccessor contextAccessor, ILogger<AdminController> logger)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _contextAccessor = contextAccessor;
            this.logger = logger;
        }

        [HttpGet]
        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// 在注册的时候，Identity默认的是英文提示，要想改成中文提示，继承IdentityErrorDescriber接口，注册实现类的依赖
        /// </summary>
        /// <param name="registerViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid) 
            {
                var user = new ApplicationUser {
                    UserName = registerViewModel.Email,
                    Email=registerViewModel.Email,
                    City=registerViewModel.City
                };
                var result = await userManager.CreateAsync(user, registerViewModel.Password);
                if (result.Succeeded) {

                    //生成电子邮箱确认令牌
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    //生成电子邮箱的确认链接
                    var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = token }, Request.Scheme);
                    logger.Log(LogLevel.Warning, confirmationLink);



                    //如果用户登录为Admin，创建的角色后重定向到ListUsers的视图列表
                    if (signInManager.IsSignedIn(User)&&User.IsInRole("Admin")) {
                        return RedirectToAction("ListUsers", "Admin");
                    }
                    ViewBag.ErrorTitle = "注册成功";
                    ViewBag.ErrorMessage = $"在您登录系统之前，我们已经向您的邮箱发送了一份邮件，需要您进行验证，点击确认链接即可完成身份验证!";
                    return View("Error");

                    ////否则重定向到Student控制器的Index页面
                    //await signInManager.SignInAsync(user, isPersistent: false);
                    //return RedirectToAction("Index", "Student");

                }
                foreach (var error in result.Errors) {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(registerViewModel);
        }

        [Route("Logout")]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Student");
        }

        [Route("Login")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl)
        {
            LoginViewModel model = new LoginViewModel {
                ReturnUrl = returnUrl,
                ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            return View(model);
        }

        [Route("Login")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel,string returnUrl)
        {
            loginViewModel.ExternalLogins= (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid) {
                var user=await userManager.FindByEmailAsync(loginViewModel.Email);
                if (user!=null&&!user.EmailConfirmed&&(await userManager.CheckPasswordAsync(user,loginViewModel.Password))) {
                    ModelState.AddModelError(string.Empty, "Email not confirmed yet");
                    return View(loginViewModel);
                }

                //将PasswordSignInAsync 最后一个传参置为true，启动账户锁定功能

                var loginResult = await signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, loginViewModel.RememberMe, true);
                if (loginResult.Succeeded) {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) {
                        return Redirect(returnUrl);
                    }
                    else {
                        return RedirectToAction("Index", "Student");
                    }
                }
                if (loginResult.IsLockedOut) {
                    return View("AccountLocked");   //如果账户锁定，返回账户锁定视图
                }
                ModelState.AddModelError(string.Empty, "登录失败，请重试!");
            }
            return View(loginViewModel);
        }


        [Route("ExternalLogin")]
        [HttpPost]
        //[AllowAnonymous]
        public IActionResult ExternalLogin(string provider,string returnUrl)
        {
            var redirecrUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });

            var request = _contextAccessor.HttpContext.Request;
            var url =
                $"{request.Scheme}://{request.Host}{request.PathBase}{request.Path}-callback?provider={provider}&redirectUrl={returnUrl}";

            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirecrUrl);
            return new ChallengeResult(provider, properties);
        }

        [HttpGet]
        [Route("ExternalLoginCallback")]
        //[AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl=null,string remoteError=null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            LoginViewModel loginViewModel = new LoginViewModel {
                ReturnUrl = returnUrl,
                ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList(),
            };
            if (remoteError != null) {
                ModelState.AddModelError(string.Empty, $"第三方登录提供程序错误:{remoteError}");
                return View("Login", loginViewModel);
            }
            //从第三方登录程序，获取用户登录信息
            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null) {
                ModelState.AddModelError(string.Empty, "加载第三方登录信息出错");
                return View("Login", loginViewModel);
            }
            var signInResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (signInResult.Succeeded) {
                return LocalRedirect(returnUrl);
            }
            else {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                if (email != null) {
                    var user = await userManager.FindByEmailAsync(email);
                    if (user == null) {
                        user = new ApplicationUser {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                        };
                        await userManager.CreateAsync(user);    //如果不存在，创建一个用户,没有密码
                    }
                    await userManager.AddLoginAsync(user, info);
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                //如果获取不到邮箱地址，重定向到错误视图
                ViewBag.ErrorTitle = $"无法从第三方登录平台:{info.LoginProvider}中解析用户的邮箱地址";
                ViewBag.ErrorMessage = "请寻求技术支持";
                return View("Error");
            }
        }


        [AcceptVerbs("Get","Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user==null) {
                return Json(true);
            }
            else {
                return Json($"邮箱:{email} 已经被注册使用!");
            }

        }

        [HttpGet]
        [Route("ConfirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId,string token)
        {
            if (userId==null||token==null) {
                return RedirectToAction("Index", "Student");
            }
            var user=await userManager.FindByIdAsync(userId);
            if (user==null) {
                ViewBag.ErrorMessage = $"当前{userId}无效";
                return View("NotFound");
            }
            var result=await userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded) {
                return View();
            }
            ViewBag.ErrorTitle = "您的电子邮件还未进行验证";
            return RedirectToAction("Error");
        }


        #region 激活账户功能
        [HttpGet]
        [Route("ActivateUserEmail")]
        public IActionResult ActivateUserEmail()
        { 
            return View();
        }

        [HttpPost]
        [Route("ActivateUserEmail")]
        public async Task<IActionResult> ActivateUserEmail(EmailAddressViewModel model)
        {
            if (ModelState.IsValid) {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user!=null) {
                    if (!await userManager.IsEmailConfirmedAsync(user)) {
                        var token=await userManager.GenerateEmailConfirmationTokenAsync(user);
                        var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = token }, Request.Scheme);
                        logger.Log(LogLevel.Warning,confirmationLink);
                        ViewBag.Message = "如果您在系统中已经注册了账户，我们已经发送了邮件到您的邮箱中，请前往邮箱激活您的账户!";
                        return View("ActivateUserEmailConfirmation", ViewBag.Message);
                    }
                }
            }
            ViewBag.Message = "请确认邮箱是否存在异常，我们无法发送邮件到您的邮箱中!";
            return View("ActivateUserEmailConfirmation", ViewBag.Message);
        }
        #endregion

        #region 忘记密码重置密码功能

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
            if (ModelState.IsValid) {
                var user=await userManager.FindByEmailAsync(model.Email);
                if (user!=null&&await userManager.IsEmailConfirmedAsync(user)) {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var passwordResetLink = Url.Action("ResetPassword", "Account", new { email=model.Email,token=token},Request.Scheme);
                    logger.Log(LogLevel.Warning,passwordResetLink);
                    //重定向到忘记密码确认视图
                    return View("ForgotPasswordConfirmation");
                }
                return View("ForgotPasswordConfirmation");
            }
            return View(model);
        }

        [HttpGet]
        [Route("ResetPassword")]
        public IActionResult ResetPassword(string token,string email)
        {
            if (token==null||email==null) {
                ModelState.AddModelError("", "无效的密码重置令牌");
            }
            return View();
        }

        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid) {
                var user= await userManager.FindByEmailAsync(model.Email);
                if (user!=null) {
                    var result =await userManager.ResetPasswordAsync(user,model.Token,model.Password);
                    if (result.Succeeded) {

                        if (await userManager.IsLockedOutAsync(user)) {
                            await userManager.SetLockoutEndDateAsync(user,DateTimeOffset.UtcNow);   //如果重置密码，则解锁账户
                        }

                        return View("ResetPasswordConfirmation");
                    }
                    foreach (var error in result.Errors) {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
                return View("ResetPasswordConfirmation");
            }
            return View(model);
        }

        #endregion

        #region 修改密码

        [HttpGet]
        [Route("ChangePassword")]
        public IActionResult ChangePassword()
        {
            return View();
        }


        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid) {
                var user=await userManager.GetUserAsync(User);
                if (user==null) {
                    return RedirectToAction("Login");
                }
                var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!result.Succeeded) {
                    foreach (var error in result.Errors) {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }
                await signInManager.RefreshSignInAsync(user);
                return View("ChangePasswordConfirmation");
            }
            return View(model);
        }

        #endregion

    }
}
