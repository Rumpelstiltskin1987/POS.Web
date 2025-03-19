using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Entities
{
    public  class Stock
    {
        [Key]
        public int IdStock { get; set; }

        [Required(ErrorMessage = "El Almacén es obligatorio.")]
        [ForeignKey("Warehouse")]
        public int IdWarehouse { get; set; }
        public virtual Warehouse? Warehouse { get; set; }

        [Required(ErrorMessage = "El Producto es obligatorio.")]
        [ForeignKey("Product")]
        public int IdProduct { get; set; }
        public virtual Product? Product { get; set; }

        [Required(ErrorMessage = "La Cantidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "El stock debe ser mayor que cero.")]
        public int Quantity { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; } = DateTime.Now;
        public string? LastUpdateUser { get; set; }
        public DateTime? LastUpdateDate { get; set; }
    }
}
