using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS.Entities
{
    public class Warehouse
    {
        [Key]
        public int IdWarehouse { get; set; }
        [Required(ErrorMessage = "El Nombre es obligatorio."), StringLength(100)]
        public string? Name { get; set; }

        [Required(ErrorMessage = "La Dirección es obligatoria.")]
        [ForeignKey("WarehouseLocation")]
        public int IdWL { get; set; } 
        public virtual WarehouseLocation? WarehouseLocation { get; set; } 
        public string? CreateUser { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public string? LastUpdateUser { get; set; }
        public DateTime? LastUpdateDate { get; set; }
    }
}
    