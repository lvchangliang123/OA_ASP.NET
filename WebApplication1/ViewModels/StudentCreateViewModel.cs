using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WebApplication1.Models.EnumTypes;

namespace WebApplication1.ViewModels
{
    public class StudentCreateViewModel
    {
        //public int Id { get; set; }
        [Display(Name = "名字")]
        [Required(ErrorMessage = "请输入名字，名字不能为空"), MaxLength(50, ErrorMessage = "名字长度不能超过50个字符")]
        public string Name { get; set; }
        [Display(Name = "主修科目")]
        [Required(ErrorMessage = "请选择一门科目")]
        public MajorEnum? Major { get; set; }
        [Display(Name = "电子邮箱")]
        [Required(ErrorMessage = "请输入电子邮箱，电子邮箱不能为空")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "邮箱格式不正确")]
        public string Email { get; set; }
        //[Display(Name="Photo")]
        //public IFormFile Photo { get; set; }
        [Display(Name = "Photos")]
        public List<IFormFile> Photos { get; set; }
    }
}
