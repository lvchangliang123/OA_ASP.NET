using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels
{
    public class EmailAddressViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
