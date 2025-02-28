using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Entities
{
    public class Inventory
    {
        public int IdMovement { get; set; }
        public int IdStock { get; set; }
        public string? MovementType { get; set; }       
        public int Quantity { get; set; }
        public string? Description { get; set; }
        public string? MovementUser { get; set; }        
        public DateTime? MovementDate { get; set; }
    }
}
