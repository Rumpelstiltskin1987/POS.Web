using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Entities
{
    public  class Stock
    {
        [Key]
        public int IdStock { get; set; }

        [Required]
        public int IdWarehouse { get; set; }

        [Required]
        public int IdProduct { get; set; }

        [Required]
        public int IdCategory { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "El stock debe ser mayor que cero.")]
        public int Quantity { get; set; }
        public string? CreateUser { get; set; }
        public string? CreateDate { get; set; }
        public string? LastUpdateUser { get; set; }
        public string? LastUpdateDate { get; set; }
    }
}
