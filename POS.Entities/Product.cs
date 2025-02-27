using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Entities
{
    public class Product
    {
        [Key]
        public int IdProduct { get; set; }

        [Required(ErrorMessage = "El Nombre es obligatorio."), StringLength(100)]
        public string? Name { get; set; }
        public string? Description { get; set; }    
               
        [Required(ErrorMessage = "La categoría es obligatoria.")]
        [ForeignKey("Category")]
        public int IdCategory { get; set; }
        public Category? Category { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero.")]
        public decimal Price { get; set; }
        public string? MeasureUnit { get; set; }
        public string? UrlImage { get; set; }
        public string? Status { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? LastUpdateUser { get; set; }
        public DateTime? LastUpdateDate { get; set; }
    }
}
