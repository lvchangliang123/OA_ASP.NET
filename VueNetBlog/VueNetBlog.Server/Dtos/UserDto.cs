using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VueNetBlog.Server.Dtos
{
    public class UserDto
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Birthday { get; set; }

        public IFormFile Avatar { get; set; }
    }
}
