using BlogModels.Dtos;
using Microsoft.AspNetCore.Identity;

namespace BlogModels.ModelHelpers
{
    public class CustomerIdentityUser : IdentityUser
    {
        public DateTime Birthday { get; set; }
        public string Avatar { get; set; }
        public ICollection<BlogCollection> BlogCollections { get; set; }
    }
}
