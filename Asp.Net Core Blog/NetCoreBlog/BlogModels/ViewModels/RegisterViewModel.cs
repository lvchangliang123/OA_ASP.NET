using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http;

namespace BlogModels.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name ="用户名称")]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "邮箱地址")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name ="密码")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password",ErrorMessage ="密码不一致，请重新输入")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name ="出生日期")]
        public DateTime BirthDay { get; set; }

        [Display(Name ="用户头像")]
        public IFormFile Avatar { get; set; }



    }
}
