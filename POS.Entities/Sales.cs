using System;
using System.ComponentModel.DataAnnotations;

namespace POS.Entities
{
    public class Sales
    {
        [Key]
        public int IdSales { get; set; }

        [Required]
        public DateTime SalesDate { get; set; }

        [Required]
        public decimal Total { get; set; }

        public string CreateUser { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;

        public string LastUpdateUser { get; set; }

        public DateTime? LastUpdateDate { get; set; }
    }
}
