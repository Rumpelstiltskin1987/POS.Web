using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace POS.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        [Key]
        public int IdUser { get; set; }
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? UserPassword { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? SecondName {  get; set; }    
        public string? Email { get; set; }
        public string? Phone { get; set; }        
    }
}
