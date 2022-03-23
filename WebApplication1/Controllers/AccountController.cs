using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    [Route("Account")]
    //[AllowAnonymous]
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
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
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Student");
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
        //[AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            //if (!string.IsNullOrEmpty(returnUrl) && returnUrl!="/" && Url.IsLocalUrl(returnUrl)) {
            //    return Redirect(returnUrl);
            //}
            return View();
        }

        [Route("Login")]
        [HttpPost]
        //[AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel,string returnUrl)
        {
            if (ModelState.IsValid) {
                var loginResult = await signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, loginViewModel.RememberMe, false);
                if (loginResult.Succeeded) {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) {
                        return Redirect(returnUrl);
                    }
                    else {
                        return RedirectToAction("Index", "Student");
                    }
                }
                ModelState.AddModelError(string.Empty, "登录失败，请重试!");
            }
            return View(loginViewModel);
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

    }
}
