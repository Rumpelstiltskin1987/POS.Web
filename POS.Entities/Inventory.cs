using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Entities
{
    public class Inventory
    {
        public int IdMovement { get; set; }
        public int IdProduct { get; set; }
        public string MovementType { get; set; }
        public int Quantity { get; set; }
        public string MovementUser { get; set; }        
        public DateTime MovementDate { get; set; }
    }
}
