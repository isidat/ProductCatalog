using System;
using System.ComponentModel.DataAnnotations;

using ProductCatalog.Mvc.Extensions;

namespace ProductCatalog.Mvc.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Photo")]
        public string Photo { get; set; }

        [Required]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Display(Name = "Last Updated")]
        public DateTime LastUpdated { get; set; }

        public string PriceStr
        {
            get
            {
                return string.Format("{0:C2}", this.Price);
            }
        }

        public string LastUpdatedStr
        {
            get
            {
                return this.LastUpdated.TimeAgo();
            }
        }
    }
}