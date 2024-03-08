using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace AnyaStore.Services.ShoppingCartAPI.Models.DTO
{
    public class ProductDTO
    {
        public int? ProductId { get; set; }
        public int? CategoryId { get; set; } //foreign key
        public string? Name { get; set; }
        public double? Price { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        // public CategoryDTO? Category { get; set; } // included property

    }
}