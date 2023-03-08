using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BlogModels.ViewModels
{
    public class EditUserInfoViewModel
    {
        [Required]
        [Display(Name = "用户名称")]
        public string NewUserName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "邮箱地址")]
        public string NewEmail { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "用户密码")]
        public string? Password { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "出生日期")]
        public DateTime NewBirthDay { get; set; }

        [Display(Name = "用户头像")]
        public IFormFile? NewAvatar { get; set; }

        [Display(Name = "用户头像")]
        public string? OldAvatar { get; set; }
    }
}
