using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name ="角色")]
        public string RoleName { get; set; }
    }
}
