using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ViewModels
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Claims = new List<System.Security.Claims.Claim>();
            Roles = new List<string>();
        }

        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string City { get; set; }
        public IList<System.Security.Claims.Claim> Claims { get; set; }
        public IList<string> Roles { get; set; }


    }
}
