using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace AnyaStore.Services.ProductAPI.Models
{
    public class Product
    {
        [Required]
        public int ProductId { get; set; }
        public int? CategoryId { get; set; }
        public string? Name { get; set; }
        public double? Price { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime? LastUpdated { get; set; } = DateTime.Now;
        public DateTime? CreatedOn { get; set; } = DateTime.Now;

    }
}