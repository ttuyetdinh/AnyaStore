using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace AnyaStore.Services.ProductAPI.Models
{
    public class Category
    {
        [Required]
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public DateTime? LastUpdated { get; set; } = DateTime.Now;
        public DateTime? CreatedOn { get; set; } = DateTime.Now;

        public ICollection<Product> Products { get; set; } // navigation property

    }
}