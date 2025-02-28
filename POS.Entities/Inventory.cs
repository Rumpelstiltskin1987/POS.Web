using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Entities
{
    public class Inventory
    {
        public int IdMovement { get; set; }
        [Required]
        [ForeignKey("Stock")]
        public int IdStock { get; set; }
        public Stock Stock { get; set; }
        [Required]  
        public string? MovementType { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string? Description { get; set; }
        public string? MovementUser { get; set; }        
        public DateTime? MovementDate { get; set; } = DateTime.Now;
    }
}
