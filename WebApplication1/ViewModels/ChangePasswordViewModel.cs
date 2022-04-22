using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name ="当前密码：")]
        public string CurrentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "新密码：")]
        public string NewPassword { get; set; }

        [Required]
        [Display(Name ="确认新密码")]
        [Compare("NewPassword",ErrorMessage ="新密码和确认密码不匹配,请重新输入")]
        public string ConfirmPassword { get; set; }

    }
}
