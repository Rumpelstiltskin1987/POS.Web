using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace POS.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

    }
}
