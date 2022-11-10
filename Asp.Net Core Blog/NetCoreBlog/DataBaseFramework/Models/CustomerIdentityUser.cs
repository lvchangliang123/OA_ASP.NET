using Microsoft.AspNetCore.Identity;

namespace DataBaseFramework.Models
{
    public class CustomerIdentityUser : IdentityUser
    {
        public DateTime Birthday { get; set; }
        public string Avatar { get; set; }
    }
}
