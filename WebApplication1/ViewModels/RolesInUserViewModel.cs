using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ViewModels
{
    public class RolesInUserViewModel
    {
        /// <summary>
        /// 用户拥有的角色列表
        /// </summary>
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsSelected { get; set; }

    }
}
