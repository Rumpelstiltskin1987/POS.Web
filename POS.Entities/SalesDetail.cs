using System.ComponentModel.DataAnnotations;

namespace POS.Entities
{
    public class SalesDetail
    {
        [Key]
        public int IdSalesDetail { get; set; }

        public int IdSales { get; set; }

        public int IdProduct { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        [Required]
        public decimal Subtotal { get; set; }

        public string CreateUser { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;

        public string LastUpdateUser { get; set; }

        public DateTime? LastUpdateDate { get; set; }
    }
}
