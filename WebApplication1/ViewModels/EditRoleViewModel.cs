﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ViewModels
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<string>();
        }
        [Display(Name ="角色Id")]
        public string Id { get; set; }
        [Required(ErrorMessage ="角色名称不能为空")]
        [Display(Name ="角色名称")]
        public string RoleName { get; set; }
        public List<string> Users { get; set; }
    }
}
