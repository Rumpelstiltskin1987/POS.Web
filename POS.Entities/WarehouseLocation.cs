using System.ComponentModel.DataAnnotations;

namespace POS.Entities
{
    public class WarehouseLocation
    {
        [Key]
        public int IdWL { get; set; }

        [Required(ErrorMessage = "La Dirección es obligatoria."), StringLength(100)]
        public string Address { get; set; }
    }
}
