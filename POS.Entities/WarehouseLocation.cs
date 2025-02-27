using System.ComponentModel.DataAnnotations;

namespace POS.Entities
{
    public class WarehouseLocation
    {
        [Key]
        public int IdWarehouseLocation { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(200)]
        public string Address { get; set; }
    }
}
