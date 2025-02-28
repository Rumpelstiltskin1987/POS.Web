using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Entities
{
    public class ProductLog
    {
        public int IdMovement { get; set; }

        [ForeignKey("Product")]
        public int IdProduct { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        [ForeignKey("Category")]
        public int IdCategory { get; set; }
        public decimal Price { get; set; }
        public string? MeasureUnit { get; set; }
        public string? UrlImage { get; set; }
        public string? Status { get; set; }
        public string? MovementType { get; set; }
        public string? LastUpdateUser { get; set; }
        public DateTime? LastUpdateDate { get; set; }
    }
}
