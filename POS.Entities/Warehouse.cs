using System;
using System.ComponentModel.DataAnnotations;

namespace POS.Entities
{
    public class Warehouse
    {
        [Key]
        public int IdWarehouse { get; set; }

        public int IdWarehouseLocation { get; set; }

        public int IdCategory { get; set; }

        public int IdProduct { get; set; }

        [Required]
        public int Stock { get; set; }

        public string CreateUser { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;

        public string LastUpdateUser { get; set; }

        public DateTime? LastUpdateDate { get; set; }
    }
}
    