using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    /// <summary>
    /// 用于管理一些声明
    /// </summary>
    public class ClaimsStore
    {
        public static List<Claim> AllClaims = new List<Claim>() {
        new Claim("Create Role","Create Role"),
        new Claim("Edit Role","Edit Role"),
        new Claim("Delete Role","Delete Role"),
        new Claim("EditStudent","EditStudent"),
        };
    }
}
