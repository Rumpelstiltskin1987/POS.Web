using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Entities
{
    public class CategoryLog
    {
        public int IdMovement { get; set; }
        public int IdCategory { get; set; }
        public string? Name { get; set; }
        public string? Status { get; set; }
        public string? MovementType { get; set; }    
        public string? LastUpdateUser { get; set; }
        public DateTime? LastUpdateDate { get; set; } = DateTime.Now;
    }
}
