using System;
using System.ComponentModel.DataAnnotations;

namespace ProductCatalog.Data.Models.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public decimal Price { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}