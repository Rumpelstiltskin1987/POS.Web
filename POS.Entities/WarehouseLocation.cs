using System.ComponentModel.DataAnnotations;

namespace POS.Entities
{
    public class WarehouseLocation
    {
        [Key]
        public int IdWL { get; set; }

        [Required(ErrorMessage = "La Dirección es obligatoria."), StringLength(100)]
        public string Address { get; set; }
        public string? CreateUser { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public string? LastUpdateUser { get; set; }
        public DateTime? LastUpdateDate { get; set; }
    }
}
