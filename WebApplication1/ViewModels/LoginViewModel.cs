using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace WebApplication1.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name ="记住我")]
        public bool RememberMe { get; set; }
        [Description("用于身份验证之前尝试访问的URL")]
        public string ReturnUrl { get; set; }
        [Description("用于存储第三方登录")]
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

    }
}
